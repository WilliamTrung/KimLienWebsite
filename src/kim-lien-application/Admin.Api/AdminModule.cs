using Admin.Application.Abstractions;
using Admin.Infrastructure;
using Admin.Infrastructure.Data;
using Common.Api;
using Common.Api.Abstractions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Api
{
    public class AdminModule : BaseModule, IModule
    {
        public string Name => "Admin";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterInterceptors();
            AddDbContext<AdminDbContext>(services, configuration);
            services.AddScoped<IAdminDbContext>(provider => provider.GetRequiredService<AdminDbContext>());
            services.AddAutoMapper(typeof(Application.Marker).Assembly);
            services.AddValidatorsFromAssembly(typeof(Application.Marker).Assembly);
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Infrastructure.Marker).Assembly));
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
