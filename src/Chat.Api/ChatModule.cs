using Chat.Infrastructure;
using Chat.Infrastructure.Data;
using Chat.Infrastructure.DataHub;
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

            // Register SignalR
            // Note: JWT authentication is configured globally in Authen module
            // SignalR will automatically use the same JWT authentication scheme
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            })
            .AddJsonProtocol(); // Use JSON protocol for SignalR

            // Optional: Add Redis backplane for scaling (uncomment if needed)
            // var redisConnection = configuration.GetConnectionString("Redis");
            // if (!string.IsNullOrEmpty(redisConnection))
            // {
            //     services.AddSignalR()
            //         .AddStackExchangeRedis(redisConnection, options =>
            //         {
            //             options.Configuration.ChannelPrefix = "Chat";
            //         });
            // }

            // Register infrastructure services
            services.RegisterInfrastructure();

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
