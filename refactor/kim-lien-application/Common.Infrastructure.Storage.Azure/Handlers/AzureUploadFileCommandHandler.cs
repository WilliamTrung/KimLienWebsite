namespace Common.Infrastructure.Storage.Azure.Handlers
{
    public class AzureUploadFileCommandHandler : AzureStorageBaseHandler, MediatR.IRequestHandler<Commands.AzureUploadFileCommand, Common.Application.Storage.Models.CloudFileResult>
    {
        public AzureUploadFileCommandHandler(IServiceProvider service) : base(service)
        {
        }
        public async Task<Common.Application.Storage.Models.CloudFileResult> Handle(Commands.AzureUploadFileCommand request, CancellationToken cancellationToken)
        {
            return await _storageService.UploadAsync(request, cancellationToken);
        }
    }
}
