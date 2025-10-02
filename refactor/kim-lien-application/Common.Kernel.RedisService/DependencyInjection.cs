using Common.Kernel.RedisService.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Kernel.RedisService
{
    public static class DependencyInjection
    {
        public static void RegisterRedis(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddSingleton<IRedisService, RedisService.Implementations.RedisService>();
        }
    }
}
