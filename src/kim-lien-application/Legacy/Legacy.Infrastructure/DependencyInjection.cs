using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legacy.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config, string connectionString = "Legacy")
        {
            var connection = config.GetConnectionString(connectionString);
            services.AddDbContext<DatabaseContext.SqlContext>(x => x.UseNpgsql(connection));
        }
    }
}
