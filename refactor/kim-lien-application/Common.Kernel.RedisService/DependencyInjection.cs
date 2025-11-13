using Common.Kernel.RedisService.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Common.Kernel.RedisService
{
    public static class DependencyInjection
    {
        public static void RegisterRedis(this IServiceCollection services, IConfiguration cfg)
        {
            var setting = cfg.GetRequiredSection("Redis") as RedisSetting;
            if (setting is null)
            {
                throw new Exception("Redis setting invalid");
            }
            var options = new ConfigurationOptions
            {
                Password = setting.Password, // If Redis is secured
                Ssl = false, // Set to true if using SSL
            };
            options.EndPoints.Add(setting.EndPoint);
            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddSingleton<IRedisService, RedisService.Implementations.RedisService>();
        }
    }
}
