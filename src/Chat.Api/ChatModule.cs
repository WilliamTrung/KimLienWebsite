using Chat.Infrastructure;
using Chat.Infrastructure.Data;
using Common.Api;
using Common.Api.Abstractions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Api
{
    public class ChatModule : BaseModule, IModule
    {
        public string Name => "Chat";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterInterceptors();
            AddDbContext<ChatContext>(services, configuration);
            services.AddAutoMapper(typeof(Application.Marker).Assembly);
            services.AddValidatorsFromAssembly(typeof(Application.Marker).Assembly);
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Infrastructure.Marker).Assembly));

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
