using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api.Abstractions
{
    public interface IModule
    {
        string Name { get; }
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
