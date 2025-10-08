using Common.Infrastructure.Storage.Azure.Commands;
using MediatR;

namespace Common.Infrastructure.Storage.Azure.Handlers
{
    public class AzureDeleteBatchCommandHandler : AzureStorageBaseHandler, IRequestHandler<AzureDeleteBatchCommand>
    {
        public AzureDeleteBatchCommandHandler(IServiceProvider service) : base(service)
        {
        }

        public async Task Handle(AzureDeleteBatchCommand request, CancellationToken cancellationToken)
        {
            await _storageService.DeleteManyAsync(request.FileDestinations, cancellationToken);
        }
    }
}
