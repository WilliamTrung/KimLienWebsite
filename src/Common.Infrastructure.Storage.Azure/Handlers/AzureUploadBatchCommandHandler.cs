using Common.Application.Storage.Models;

namespace Common.Infrastructure.Storage.Azure.Handlers
{
    public class AzureUploadBatchCommandHandler : AzureStorageBaseHandler, MediatR.IRequestHandler<Commands.AzureUploadBatchCommand, IReadOnlyList<CloudFileResult>>
    {
        public AzureUploadBatchCommandHandler(IServiceProvider service) : base(service)
        {
        }
        public async Task<IReadOnlyList<CloudFileResult>> Handle(Commands.AzureUploadBatchCommand request, CancellationToken cancellationToken)
        {
            return await Resolve(request).UploadAsync(request, cancellationToken);
        }
    }
}
