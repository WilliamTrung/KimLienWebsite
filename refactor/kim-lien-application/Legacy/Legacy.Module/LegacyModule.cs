using Common.Api;
using Common.Api.Abstractions;
using Legacy.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Legacy.Module
{
    public class LegacyModule : BaseModule, IModule
    {
        public string Name => "Legacy";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.AddAutoMapper(typeof(Application.Marker).Assembly);
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Application.Marker).Assembly));

            // 2. Bulk conventions via Scrutor
            services.AddMarkedServices(
                 typeof(Application.Marker).Assembly
             );
            services.AddMarkedServices(
                typeof(Infrastructure.Marker).Assembly
            );
        }
    }
}
