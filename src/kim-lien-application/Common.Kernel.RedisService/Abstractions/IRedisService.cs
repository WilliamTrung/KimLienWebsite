namespace Common.Kernel.RedisService.Abstractions
{
    public interface IRedisService
    {
        Task<T> CreateOrUpdateAsync<T>(string key, T entity) where T : class;
        Task<T> UpdateAsync<T>(string key, T entity) where T : class;
        Task<T> GetByKeyAsync<T>(string key) where T : class;
        Task<IEnumerable<T>> GetValuesAsync<T>(IEnumerable<string> keys) where T : class;
        Task<bool> DeleteAsync(string key);


        Task<T?> GetAsync<T>(string key);

        Task<bool> SetAsync(string key, object value, TimeSpan? expiry = null);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null,
             bool cacheNull = false, TimeSpan? lockTtl = null, CancellationToken ct = default) where T : class;
    }
}
