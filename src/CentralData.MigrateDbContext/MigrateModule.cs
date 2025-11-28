using Common.Api;
using Common.Api.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CentralData.MigrateDbContext
{
    public class MigrateModule : BaseModule, IModule
    {
        public string Name => "CentralMigration";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext<GlobalDbContext>(services, configuration);
        }
    }
}
