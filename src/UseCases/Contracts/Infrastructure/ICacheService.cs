// namespace UseCases.Contracts.Infrastructure;
// public interface ICacheService
// {
//     Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T: class;
//     Task<T?> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class;
//     Task<bool> SetAsync<T>(string Key, T Value, TimeSpan timeSpan = default,  CancellationToken cancellationToken = default) where T: class;
// }