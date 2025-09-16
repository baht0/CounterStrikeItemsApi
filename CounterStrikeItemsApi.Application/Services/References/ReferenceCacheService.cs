using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CounterStrikeItemsApi.Application.Services.References
{
    public class ReferenceCacheService(IDistributedCache cache)
    {
        private readonly IDistributedCache _cache = cache;

        private static string BuildKey<TEntity>(string suffix) =>
            $"reference:{typeof(TEntity).Name}:{suffix}";

        public async Task<T?> GetAsync<T, TEntity>(string suffix)
        {
            var key = BuildKey<TEntity>(suffix);
            var cached = await _cache.GetStringAsync(key);
            if (cached == null) return default;
            return JsonSerializer.Deserialize<T>(cached);
        }

        public async Task SetAsync<T, TEntity>(string suffix, T value, TimeSpan? expiry = null)
        {
            var key = BuildKey<TEntity>(suffix);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(6)
            };

            var serialized = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serialized, options);
        }

        public async Task RemoveAsync<TEntity>(string suffix)
        {
            var key = BuildKey<TEntity>(suffix);
            await _cache.RemoveAsync(key);
        }
    }
}
