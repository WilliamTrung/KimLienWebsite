using Common.Api;
using Common.Api.Abstractions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Import.Api
{
    public class ImportModule : BaseModule, IModule
    {
        public string Name => "Import";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Application.Marker).Assembly);
            services.AddValidatorsFromAssembly(typeof(Application.Marker).Assembly);
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
