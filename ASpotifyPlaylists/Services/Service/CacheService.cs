using ASpotifyPlaylists.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace ASpotifyPlaylists.Services.Service
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache) 
        {
            _cache = cache;
        }
        public T? GetData<T>(Guid id)
        {
            var value = _cache.GetString(id.ToString());

            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T?>(value!);
        }

        public void SetData<T>(Guid id, T data)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.SetString(id.ToString(), JsonSerializer.Serialize(data), options);
        }

        public void RemoveData(Guid id)
        {
            _cache.Remove(id.ToString());
        }
    }
}
