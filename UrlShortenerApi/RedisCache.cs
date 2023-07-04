using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace UrlShortenerApi
{
    public class RedisService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetCachedDataAsync(string key)
        {
            var data = await _cache.GetStringAsync(key);

            if (data == null)
            {
                return null;
            }

            return data;
        }

        public void SaveToCache(string key, object data)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(30)); // Cache the data for 30 minutes

            var jsonData = JsonSerializer.Serialize(data);

            _cache.Set(key, Encoding.UTF8.GetBytes(jsonData), options);
        }
    }
}
