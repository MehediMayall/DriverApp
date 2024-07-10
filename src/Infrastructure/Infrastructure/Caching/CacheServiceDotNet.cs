using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace Infrastructure.Caching;


public class CacheService : ICacheService
{
    private readonly IDistributedCache distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        this.distributedCache = distributedCache;
    }
    public async Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await distributedCache.GetStringAsync(Key, cancellationToken);
        if (cachedValue is null) return null;

        return cachedValue.ConvertTo<T>();
    }
    public async Task<T?> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class
    {
        var cachedValue = await distributedCache.GetStringAsync(Key);
        if (!string.IsNullOrEmpty(cachedValue))
            return cachedValue.ConvertTo<T>();

        if (NonCached is null) return null;

        var nonCachedValue = await NonCached();
        await SetAsync<T>(Key, nonCachedValue, timeSpan, cancellationToken);
        return nonCachedValue;
    }

    public async Task<T?> GetAsync<T>(string Key, Func<Task<T>> NonCached, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await GetAsync<T>(Key, cancellationToken);
        if(cachedValue is not null) return cachedValue;

        var nonCachedValue = await NonCached();
        await SetAsync(Key, nonCachedValue, cancellationToken);
        return nonCachedValue; 
    }

    public async Task RemoveAsync(string Key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(Key, cancellationToken);
    }

    public async Task SetAsync<T>(string Key, T Value, CancellationToken cancellationToken = default) where T : class
    {
        string? jsonString = Value.GetJsonString<T>();
        if(jsonString is not null)
        {
            await distributedCache.SetStringAsync(Key, jsonString, cancellationToken);
        }
    }

    public async Task SetAsync<T>(string Key, T Value, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T : class
    {
        string? jsonString = Value.GetJsonString<T>();
        if(jsonString is not null)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(timeSpan);
            await distributedCache.SetStringAsync(Key, jsonString, options, cancellationToken);
        }
    }
}