using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api
{
    public abstract class BaseModule
    {
        /// <summary>
        /// Add db context
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public virtual void AddDbContext<TContext>(IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            // Register the DbContext for modules, all modules will using same as Connection
            services.AddDbContext<TContext>((sp, o) =>
            {
                o.UseNpgsql(sp.GetRequiredService<Npgsql.NpgsqlConnection>());
                o.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });
        }
    }
}
