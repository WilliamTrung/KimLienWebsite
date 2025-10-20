using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Kernel.TenantProvider
{
    public static class DependencyInjection
    {
        public static void RegisterTenantProvider(this IServiceCollection services, IConfiguration cfg)
        {
            var tenantId = cfg.GetRequiredSection("TenantProvider:TenantId").Value;
            var tenantProvider = new TenantProvider.Implementations.TenantProvider();
            tenantProvider.SetTenantId(tenantId);
            Implementations.TenantProvider.Instance = tenantProvider;
            services.AddSingleton(tenantProvider);
        }
    }
}
