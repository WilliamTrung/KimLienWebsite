using Chat.Application.Common.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Register application services here
            services.AddSingleton<Common.Abstractions.IConnectionPoolProvider, ConnectionPoolProvider>();
            services.AddSingleton<Common.Abstractions.IAnonymousConnectionPoolProvider, AnonymousConnectionPoolProvider>();
        }
    }
}
