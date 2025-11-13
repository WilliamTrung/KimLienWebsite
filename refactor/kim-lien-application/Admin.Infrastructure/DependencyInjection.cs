using Common.Infrastructure.Interceptor;
using Common.Infrastructure.Interceptor.TenantQuery;
using Common.Kernel.TenantProvider;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInterceptors(this IServiceCollection services, IConfiguration? cfg = null)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, QueryEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, TenantSaveChangeInterceptor>();
            services.RegisterTenantProvider(cfg ?? services.BuildServiceProvider().GetRequiredService<IConfiguration>());
        }
    }
}
