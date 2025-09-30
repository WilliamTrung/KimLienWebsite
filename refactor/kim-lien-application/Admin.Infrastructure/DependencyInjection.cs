using Common.Infrastructure.Interceptor;
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
        }
    }
}
