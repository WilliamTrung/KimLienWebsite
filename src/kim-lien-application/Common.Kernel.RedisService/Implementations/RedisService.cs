using Common.Extension;
using Common.Kernel.Dependencies;
using Common.Kernel.RedisService.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Common.Kernel.RedisService.Implementations
{
    public class RedisService : IRedisService, ISingleton
    {
        private readonly IDatabase _database;
        private const string UnlockScript = @"
            if redis.call('get', KEYS[1]) == ARGV[1] then
              return redis.call('del', KEYS[1])
            else
              return 0
            end";

        public RedisService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<T> CreateOrUpdateAsync<T>(string key, T entity) where T : class
        {
            var created = await _database.StringSetAsync(key,
                entity.TrySerializeObject());

            await _database.KeyPersistAsync(key);

            if (!created)
            {
                return null;
            }

            return await GetByKeyAsync<T>(key);
        }

        public async Task<T> UpdateAsync<T>(string key, T entity) where T : class
        {
            var created = await _database.StringSetAsync(key,
                entity.TrySerializeObject(), TimeSpan.FromMinutes(60));

            if (!created)
            {
                return null;
            }

            return await GetByKeyAsync<T>(key);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }


        public async Task<T> GetByKeyAsync<T>(string key) where T : class
        {
            var json = await _database.StringGetAsync(key);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<IEnumerable<T>> GetValuesAsync<T>(IEnumerable<string> keys) where T : class
        {
            var redisKeys = keys.Select(key => (RedisKey)key).ToArray();
            var redisValues = await _database.StringGetAsync(redisKeys);

            if (redisValues == null || redisValues.Length == 0)
                return Enumerable.Empty<T>();

            return redisValues.Select(x => x.ToString().TryDeserializeObject<T>()).ToList();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> SetAsync(string key, object value, TimeSpan? expiry = null)
        {
            var json = value.TrySerializeObject();
            if (expiry is null)
            {
                expiry = TimeSpan.FromMinutes(60);
            }
            var created = await _database.StringSetAsync(key, json, expiry.Value);
            return created;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null,
            bool cacheNull = false, TimeSpan? lockTtl = null, CancellationToken ct = default) where T : class
        {
            var cached = await _database.StringGetAsync(key);
            if (!cached.IsNullOrEmpty)
                return cached.ToString() == "__null__" ? null : cached.ToString().TryDeserializeObject<T>();

            // avoid stampede
            var lockKey = $"{key}:lock";
            var lockVal = Guid.NewGuid().ToString("N");
            var acquired = await _database.StringSetAsync(lockKey, lockVal, lockTtl ?? TimeSpan.FromSeconds(5), When.NotExists);
            if (!acquired)
            {
                // avoid herd (Có thread khác đang nạp cache → đợi ngắn rồi đọc lại)
                await Task.Delay(Random.Shared.Next(20, 80), ct);
                var retry = await _database.StringGetAsync(key);
                if (!retry.IsNullOrEmpty)
                    return retry.ToString() == "__null__" ? null : retry.ToString().TryDeserializeObject<T>();
            }

            try
            {
                var recheck = await _database.StringGetAsync(key);
                if (!recheck.IsNullOrEmpty)
                    return recheck.ToString() == "__null__" ? null : recheck.ToString().TryDeserializeObject<T>();

                // limit time factory để không giữ lock quá lâu
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                cts.CancelAfter(TimeSpan.FromSeconds(3)); // tune theo workload/DB
                var data = await factory().WaitAsync(TimeSpan.FromSeconds(3), cts.Token);

                TimeSpan ResolveExpiry(TimeSpan? requested, TimeSpan? fallback)
                {
                    if (requested == Timeout.InfiniteTimeSpan) return TimeSpan.FromMinutes(60);   // No TTL
                    return requested ?? fallback ?? TimeSpan.FromMinutes(60);
                }

                if (data == null)
                {
                    if (cacheNull)
                    {
                        var nullTtl = TimeSpan.FromMinutes(2);
                        await _database.StringSetAsync(key, "__null__", ResolveExpiry(ttl, nullTtl));
                    }
                    return null;
                }

                var defaultTtl = TimeSpan.FromMinutes(60);
                await _database.StringSetAsync(key, data.TrySerializeObject(), ResolveExpiry(ttl, defaultTtl));
                return data;
            }
            finally
            {
                // thu hồi lock by Lua
                await _database.ScriptEvaluateAsync(UnlockScript, new RedisKey[] { lockKey }, new RedisValue[] { lockVal });
            }
        }
    }
}
