using CounterStrikeItemsApi.Application.DTOs.Items;
using CounterStrikeItemsApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Workers
{
    public class ItemUpdateWorker : BackgroundService
    {
        private readonly ILogger<ItemUpdateWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _runLock = new(1, 1);
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly TimeSpan Interval;
        private readonly int MaxRequestsPerRun;
        private readonly int PerRequestDelayMs;

        private static DateTime Now => DateTime.Now;

        public ItemUpdateWorker(IServiceProvider serviceProvider, 
            IConfiguration configuration, 
            ILogger<ItemUpdateWorker> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory;

            MaxRequestsPerRun = configuration.GetValue<int?>("Workers:ItemUpdate:MaxRequestsPerRun") ?? 25;
            PerRequestDelayMs = configuration.GetValue<int?>("Workers:ItemUpdate:PerRequestDelayMs") ?? 100;
            var minutes = configuration.GetValue<int?>("Workers:ItemUpdate:IntervalMinutes") ?? 5;
            if (minutes <= 0) 
                minutes = 5;
            Interval = TimeSpan.FromMinutes(minutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ItemUpdateWorker started. Interval: {Interval}", Interval);

            while (!stoppingToken.IsCancellationRequested)
            {
                var taken = await _runLock.WaitAsync(0, stoppingToken);
                if (!taken)
                {
                    _logger.LogWarning("Previous run still in progress — skipping this interval.");
                }
                else
                {
                    try
                    {
                        await RunOnceAsync(stoppingToken);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        // graceful shutdown
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled error in ItemUpdateWorker run");
                    }
                    finally
                    {
                        _runLock.Release();
                    }
                }

                try
                {
                    await Task.Delay(Interval, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }

            _logger.LogInformation("ItemUpdateWorker stopped.");
        }
        private async Task RunOnceAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ItemUpdateWorker: run started at {Time}", Now);

            // create scope to resolve scoped services (repositories, dbcontext, application services)
            using var scope = _serviceProvider.CreateScope();
            var sp = scope.ServiceProvider;

            var updater = sp.GetRequiredService<IItemSteamIdUpdater>();

            //Logic
            IEnumerable<ItemWorkerUpdateDto> itemsToUpdate;
            try
            {
                itemsToUpdate = await ParseItemsToUpdateAsync(updater, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Cancelled while preparing update list.");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to prepare items to update. Skipping this run.");
                return;
            }

            if (itemsToUpdate == null || !itemsToUpdate.Any())
            {
                _logger.LogInformation("No items to update in this run.");
                return;
            }

            //Save in DB
            try
            {
                await updater.UpdateItemsPartialAsync(itemsToUpdate, cancellationToken);
                _logger.LogInformation("ItemUpdateWorker: updated {Count} items.", itemsToUpdate.Count());
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Item updates cancelled by token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during UpdateItemsPartialAsync.");
            }
            finally
            {
                _logger.LogInformation("ItemUpdateWorker: run finished at {Time}", Now);
            }
        }        

        // Главный метод
        private async Task<List<ItemWorkerUpdateDto>> ParseItemsToUpdateAsync(IItemSteamIdUpdater updater, CancellationToken ct)
        {
            var items = (await updater.GetUpdateItemsAsync()).ToList();
            if (items.Count == 0) return [];

            var normalCat = Guid.Parse("470fe62a-aadc-4786-a50a-a18bf679cf99");
            var groups = items
                .OrderBy(x => x.CategoryId == normalCat ? 0 : 1)
                .GroupBy(i => (i.ItemCommonId, i.ExteriorId, i.QualityId))
                .Select(g => new
                {
                    g.Key,
                    Members = g.ToList(),
                    Representative = g.FirstOrDefault(x => x.ItemNameId == null) ?? g.First()
                })
                .ToList();

            using var client = _httpClientFactory.CreateClient("steam");
            var results = new List<ItemWorkerUpdateDto>();
            var processedIds = new HashSet<Guid>();
            int requestsUsed = 0;

            foreach (var group in groups)
            {
                if (ct.IsCancellationRequested)
                    break;
                if (requestsUsed >= MaxRequestsPerRun)
                {
                    _logger.LogInformation("Request quota reached ({requestsUsed}/{MaxRequestsPerRun}) — stopping collection for this run.", requestsUsed, MaxRequestsPerRun);
                    break;
                }

                var rep = group.Representative;
                _logger.LogInformation("Fetching '{Name}' for group \"ItemCommonId\"='{ItemCommonId}'", rep.Name, rep.ItemCommonId);

                string html;
                try
                {
                    var url = $"market/listings/730/{Uri.EscapeDataString(rep.Name)}";
                    using var resp = await client.GetAsync(url, ct);
                    requestsUsed++;

                    if (resp.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        // Respect Retry-After if present, then stop the run.
                        if (resp.Headers.RetryAfter != null)
                        {
                            if (resp.Headers.RetryAfter.Delta.HasValue)
                            {
                                var wait = resp.Headers.RetryAfter.Delta.Value;
                                _logger.LogWarning("Received 429; waiting {Wait} before stopping run.", wait);
                                try { await Task.Delay(wait, ct); } catch { }
                            }
                        }
                        _logger.LogWarning("Received 429 TooManyRequests. Stopping collection for this run.");
                        break;
                    }
                    if (!resp.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Non-success {StatusCode}. Skipping group.", resp.StatusCode);
                        await Task.Delay(PerRequestDelayMs, ct);
                        continue;
                    }
                    html = await resp.Content.ReadAsStringAsync(ct);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "HTTP request failed. Skipping group.");
                    await Task.Delay(PerRequestDelayMs, ct);
                    continue;
                }

                var hasImageId = TryParseImageId(html, out var imageId);
                var hasItemNameId = TryParseItemNameId(html, out var itemNameId);

                if (hasImageId || hasItemNameId)
                {
                    foreach (var member in group.Members)
                    {
                        if (processedIds.Contains(member.Id))
                            continue;

                        var dto = new ItemWorkerUpdateDto
                        {
                            Id = member.Id,
                            Name = member.Name,
                            ImageId = imageId,
                            ItemNameId = rep.Id == member.Id ? itemNameId : member.ItemNameId
                        };
                        results.Add(dto);
                        processedIds.Add(member.Id);
                    }
                }
                else
                {
                    var isNoListing = IsNoListing(html);
                    if (isNoListing)
                    {
                        var dto = new ItemWorkerUpdateDto
                        {
                            Id = rep.Id,
                            Name = rep.Name,
                            ImageId = "-",
                            ItemNameId = 0
                        };
                        results.Add(dto);
                        processedIds.Add(rep.Id);
                    }
                }
                await Task.Delay(PerRequestDelayMs, ct);
            }

            _logger.LogInformation("Collected {Count} DTOs, used {requestsUsed} HTTP requests this run.", results.Count, requestsUsed);
            return results;
        }
        private bool TryParseImageId(string html, out string? imageId)
        {
            imageId = null;
            try
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);
                var imgNode = htmlDoc.DocumentNode
                        .SelectSingleNode("//div[@class='market_listing_largeimage'][1]/img[1]");

                if (imgNode != null)
                {
                    var src = imgNode.GetAttributeValue("src", string.Empty);
                    imageId = src
                        .Replace("https://community.fastly.steamstatic.com/economy/image/", string.Empty)
                        .Replace("/360fx360f", string.Empty);

                    return true;
                }
                _logger.LogWarning($"Image not found");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unexpected error while extracting ImageId");
                return false;
            }
        }
        private bool TryParseItemNameId(string html, out int? itemNameId)
        {
            itemNameId = null;
            try
            {
                var marker = "Market_LoadOrderSpread";
                var startIndex = html.IndexOf(marker);
                if (startIndex == -1)
                {
                    _logger.LogWarning("Marker ItemNameId not found in HTML");
                    return false;
                }
                var segment = html[startIndex..];

                var openParen = segment.IndexOf('(');
                var closeParen = segment.IndexOf(')');
                if (openParen == -1 || closeParen == -1 || closeParen <= openParen)
                {
                    _logger.LogWarning("Parentheses not found in HTML segment");
                    return false;
                }
                var insideParens = segment.Substring(openParen, closeParen);

                if (int.TryParse(Regex.Replace(insideParens, @"\D+", ""), out var id))
                {
                    itemNameId = id;
                    return true;
                }

                _logger.LogWarning("Failed to parse ItemNameId");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unexpected error while extracting ItemNameId");
                return false;
            }
        }
        private bool IsNoListing(string html)
        {
            try
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);
                var node = htmlDoc.DocumentNode
                        .SelectSingleNode("//div[@class='market_listing_table_message']");

                return node != null && node.InnerText.Contains("There are no listings for this item.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unexpected error while checking listings for this item.");
                return false;
            }
        }
    }
}