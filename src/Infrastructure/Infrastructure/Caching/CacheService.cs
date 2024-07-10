
// using System.Linq.Expressions;
// using Microsoft.Extensions.Caching.Distributed;
// using StackExchange.Redis;
 
// namespace Infrastructure.Infrastructure;

// public class CacheService: ICacheService
// {
//     private readonly IDatabase cacheDB; 
//     public CacheService()
//     {
//         var redis = ConnectionMultiplexer.Connect("localhost:6379");
//         cacheDB = redis.GetDatabase();
//     }

//     public async Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T: class
//     {
//         var cachedValue = await cacheDB.StringGetAsync(Key);
//         return JsonSerializer.Deserialize<T>(cachedValue);
//     }

//     public async Task<T?> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class
//     {
//         var cachedValue = await cacheDB.StringGetAsync(Key);
//         if (!string.IsNullOrEmpty(cachedValue))
//             return JsonSerializer.Deserialize<T>(cachedValue);

//         var nonCachedValue = await NonCached();
//         await SetAsync<T>(Key, nonCachedValue, timeSpan);
//         return nonCachedValue;
//     }

//     public async Task<bool> SetAsync<T>(string Key, T Value, TimeSpan timeSpan = default,  CancellationToken cancellationToken = default) where T: class
//     {
//         return await cacheDB.StringSetAsync(Key, JsonSerializer.Serialize(Value), timeSpan);
//     }



// }