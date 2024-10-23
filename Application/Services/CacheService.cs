using System;
using System.Threading.Tasks;
using dotnetcoreproject.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace dotnetcoreproject.Application.Services;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, int cacheDuration)
        where T : class
    {
        if (!memoryCache.TryGetValue(cacheKey, out T cacheEntry))
        {
            cacheEntry = await getItemCallback();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDuration)
            };

            memoryCache.Set(cacheKey, cacheEntry, cacheEntryOptions);
        }

        return cacheEntry;
    }

    public void Remove(string cacheKey)
    {
        memoryCache.Remove(cacheKey);
    }
}