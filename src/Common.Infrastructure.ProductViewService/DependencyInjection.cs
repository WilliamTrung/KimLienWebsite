using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.ProductViewService
{
    public static class DependencyInjection
    {
        public static void RegisterProductViewService(this IServiceCollection services, IConfiguration? cfg = null)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        }
    }
}
