using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Legacy.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config, string connectionString = "Legacy")
        {
            services.AddDbContext<DatabaseContext.SqlContext>(x => x.UseSqlServer(config.GetConnectionString(connectionString)));
        }
    }
}
