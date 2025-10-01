using Authen.Infrastructure.Data;
using Common.Api;
using Common.Api.Abstractions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authen.Api
{
    public class AuthModule : BaseModule, IModule
    {
        public string Name => "Auth";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext<AuthenIdentityDbContext>(services, configuration);
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
