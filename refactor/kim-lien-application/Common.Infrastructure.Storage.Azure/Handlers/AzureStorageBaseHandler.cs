using Common.Application.Storage.Abstraction;
using Common.Infrastructure.Storage.Azure.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Storage.Azure.Handlers
{
    public abstract class AzureStorageBaseHandler
    {
        protected readonly ICloudStorageService _storageService;
        public AzureStorageBaseHandler(IServiceProvider service)
        {
            _storageService = service.GetRequiredKeyedService<ICloudStorageService>(nameof(AzureStorageService));
        }
    }
}
