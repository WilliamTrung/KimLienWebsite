using Common.Infrastructure.Storage.Azure.Commands;

namespace Common.Infrastructure.Storage.Azure.Handlers
{
    public class AzureDeleteFileCommandHandler : AzureStorageBaseHandler, MediatR.IRequestHandler<Commands.AzureDeleteFileCommand>
    {
        public AzureDeleteFileCommandHandler(IServiceProvider service) : base(service)
        {
        }

        public async Task Handle(AzureDeleteFileCommand request, CancellationToken cancellationToken)
        {
            await _storageService.DeleteAsync(request.FileDestination, cancellationToken);
        }
    }
}
