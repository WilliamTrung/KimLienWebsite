using Common.Application.Storage.Abstraction;
using Common.Infrastructure.Storage.Azure.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Storage.Azure.Handlers
{
    public abstract class AzureStorageBaseHandler(IServiceProvider service)
    {
        private ICloudStorageService? _storageServiceCache;
        protected ICloudStorageService Resolve(IAzureCommand command)
        {
            if (_storageServiceCache is null)
            {
                _storageServiceCache = service.GetKeyedService<ICloudStorageService>(command.ProfileKey)
                        ?? throw new InvalidOperationException($"Cloud storage service with key '{command.ProfileKey}' is not registered."); 
            }
            return _storageServiceCache;
        }
    }
}
