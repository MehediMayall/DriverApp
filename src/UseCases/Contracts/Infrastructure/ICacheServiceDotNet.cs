namespace UseCases.Contracts.Infrastructure;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T: class;
    // Task<T?> GetAsync<T>(string Key, Func<Task<T>> NonCached, CancellationToken cancellationToken = default) where T: class;
    Task<T?> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class;
    Task SetAsync<T>(string Key, T Value, CancellationToken cancellationToken = default) where T: class;
    Task SetAsync<T>(string Key, T Value, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string Key, CancellationToken cancellationToken = default);
}