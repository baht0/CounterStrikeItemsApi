using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System.Diagnostics.Metrics;
using WebAdminPanel.Contracts.Api;
using WebAdminPanel.Contracts.Api.References;
using WebAdminPanel.Models;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.ItemCommons;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.Models.DTOs.Reference.Collection;
using WebAdminPanel.Models.DTOs.Reference.ItemType;
using WebAdminPanel.Models.DTOs.Reference.Subtype;

namespace WebAdminPanel.Pages
{
    public partial class Items : ComponentBase
    {
        private bool _isInitialized = false;
        private bool _isFirstLoad = true;
        protected bool isLoading = true;
        private bool _expanded;
        private int selectedRowNumber = -1;
        private Guid? expandedId;
        private MudTable<ItemCommonFilteredDto> table = null!;

        protected PagedResult<ItemCommonFilteredDto> ItemCommons = new();
        protected List<CollectionDto> Collections = [];
        protected List<ItemTypeDto> Types = [];
        protected List<SubtypeDto> Subtypes = [];
        protected List<ReferenceColorDto> Qualities = [];

        private ItemCommonFilterQuery Query = new();

        //filter
        private string SearchText { get; set; } = string.Empty;

        private MudAutocomplete<ItemTypeDto> typesAutocomplete = default!;
        private List<ItemTypeDto> _selectedTypes = [];
        private MudAutocomplete<SubtypeDto> subtypeAutocomplete = default!;
        private List<SubtypeDto> _selectedSubtypes = [];
        private MudAutocomplete<ReferenceColorDto> qualityAutocomplete = default!;
        private List<ReferenceColorDto> _selectedQualities = [];
        private MudAutocomplete<CollectionDto> collectionAutocomplete = default!;
        private List<CollectionDto> _selectedCollections = [];

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            try
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Collections = await RefApiFactory.GetClient<ICollectionApi>().GetAll();
                Types = await RefApiFactory.GetClient<IItemTypeApi>().GetAll();
                Subtypes = await RefApiFactory.GetClient<ISubtypeApi>().GetAll();
                Qualities = await RefApiFactory.GetClient<IQualityApi>().GetAll();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Initialization error", Severity.Error);
                Console.WriteLine($"Error init: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryParams = QueryHelpers.ParseQuery(uri.Query);

            if (queryParams.TryGetValue("page", out var pageValue) && int.TryParse(pageValue, out var page))
            {
                Query.Page = page;
            }
            if (queryParams.TryGetValue("pageSize", out var pageSizeValue) && int.TryParse(pageSizeValue, out var pageSize))
            {
                Query.PageSize = pageSize;
                table?.SetRowsPerPage(Query.PageSize);
            }
            if (queryParams.TryGetValue("search", out var searchValue))
                Query.Search = searchValue!;
            if (queryParams.TryGetValue("types", out var types))
            {
                _selectedTypes = types
                    .Where(t => t != null)
                    .Select(t => Types.FirstOrDefault(x => x.Slug == t))
                    .Where(x => x != null)!
                    .ToList()!;
                Query.Types = [.. _selectedTypes.Select(x => x.Slug)];
            }

            if (queryParams.TryGetValue("subtypes", out var subtypes))
            {
                _selectedSubtypes = subtypes
                    .Where(t => t != null)
                    .Select(t => Subtypes.FirstOrDefault(x => x.Slug == t))
                    .Where(x => x != null)!
                    .ToList()!;
                Query.Subtypes = [.. _selectedSubtypes.Select(x => x.Slug)];
            }

            if (queryParams.TryGetValue("qualities", out var qualities))
            {
                _selectedQualities = qualities
                    .Where(t => t != null)
                    .Select(t => Qualities.FirstOrDefault(x => x.Slug == t))
                    .Where(x => x != null)!
                    .ToList()!;
                Query.Qualities = [.. _selectedQualities.Select(x => x.Slug)];
            }

            if (queryParams.TryGetValue("collections", out var collections))
            {
                _selectedCollections = collections
                    .Where(t => t != null)
                    .Select(t => Collections.FirstOrDefault(x => x.Slug == t))
                    .Where(x => x != null)!
                    .ToList()!;
                Query.Collections = [.. _selectedCollections.Select(x => x.Slug)];
            }

            _isInitialized = true; // теперь можно загружать данные

            if (table is not null)
                await table.ReloadServerData();
        }

        private string BuildQuery(ItemCommonFilterQuery query)
        {
            var queryParams = new List<string>
            {
                $"page={query.Page}",
                $"pageSize={query.PageSize}"
            };

            if (!string.IsNullOrWhiteSpace(query.Search))
                queryParams.Add($"search={Uri.EscapeDataString(query.Search)}");

            AddParams(queryParams, "collections", Query.Collections);
            AddParams(queryParams, "types", Query.Types);
            AddParams(queryParams, "subtypes", Query.Subtypes);
            AddParams(queryParams, "qualities", Query.Qualities);

            return "items?" + string.Join("&", queryParams);
        }
        private static void AddParams(List<string> queryParams, string key, IEnumerable<string>? values)
        {
            if (values != null)
            {
                foreach (var v in values)
                    queryParams.Add($"{key}={Uri.EscapeDataString(v)}");
            }
        }

        private async Task<TableData<ItemCommonFilteredDto>> LoadServerData(TableState state, CancellationToken token)
        {
            isLoading = true;

            try
            {
                if (!_isInitialized)
                    return new TableData<ItemCommonFilteredDto>();

                if (!_isFirstLoad)
                {
                    Query.Page = state.Page + 1;
                    Query.PageSize = state.PageSize;
                }
                else
                {
                    state.Page = Query.Page - 1;
                    table.SetRowsPerPage(Query.PageSize);
                    _isFirstLoad = false;
                }

                // Обновляем URL без перезагрузки страницы
                var newUri = BuildQuery(Query);
                NavigationManager?.NavigateTo(newUri, forceLoad: false);
                var result = await RefApiFactory.GetClient<IItemCommonApi>().GetSearch(Query);

                //sortiring
                var items = state.SortLabel switch
                {
                    "name_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.Name)],
                    "type_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.Type?.Name)],
                    "subtype_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.Subtype?.Name)],
                    "coll_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.Collection?.Name)],
                    "cont_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.FoundIn)],
                    "var_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.Variants)],
                    _ => result.Rows,
                };

                return new TableData<ItemCommonFilteredDto>
                {
                    Items = items,
                    TotalItems = result.TotalRows
                };
            }
            catch (ApiBadRequestException ex)
            {
                Snackbar.Add($"Bad request: {ex.Message}", Severity.Error);
                Console.WriteLine(ex.Message);
                return new();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Unexpected error: {ex.Message}", Severity.Error);
                Console.WriteLine(ex);
                return new();
            }
            finally
            {
                isLoading = false;
            }
        }
        private async Task ReloadTable()
        {
            Query = new()
            {
                Search = SearchText,
                Page = 1,
                Collections = [.. _selectedCollections.Select(s => s.Slug)],
                Types = [.. _selectedTypes.Select(s => s.Slug)],
                Subtypes = [.. _selectedSubtypes.Select(s => s.Slug)],
                Qualities = [.. _selectedQualities.Select(s => s.Slug)]
            };
            await table.ReloadServerData();
            Snackbar.Add("The filters are applied and the table reloaded.", Severity.Success);
        }
        private async Task ClearFilters()
        {
            _expanded = false;
            SearchText = string.Empty;
            _selectedTypes = [];
            _selectedSubtypes = [];
            _selectedQualities = [];
            _selectedCollections = [];

            Query = new();
            Snackbar.Add("Filters cleared.", Severity.Info);
            await table.ReloadServerData();
        }

        private string SelectedRowClassFunc(ItemCommonFilteredDto element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                expandedId = null;
                return string.Empty;
            }
            else if (table.SelectedItem != null && table.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
                expandedId = table.SelectedItem.Id;
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }
        private void RowClickEvent(TableRowClickEventArgs<ItemCommonFilteredDto> tableRowClickEventArgs) => Console.WriteLine("Row has been clicked");

        //chip_set
        private void OnExpandCollapseClick() => _expanded = !_expanded;
        private static string SlugToName(string? slug, IEnumerable<ReferenceDto> source) 
            => source.FirstOrDefault(c => c.Slug == slug)?.Name ?? slug ?? string.Empty;


        public string? ProcessedImageUrl(string? imageId) =>
            string.IsNullOrWhiteSpace(imageId) ? null : $"https://community.akamai.steamstatic.com/economy/image/{imageId}/260x260";
    }
}
