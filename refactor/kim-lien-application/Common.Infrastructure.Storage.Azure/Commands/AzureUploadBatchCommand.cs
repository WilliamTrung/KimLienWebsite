using Common.Application.Storage.Models;
using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureUploadBatchCommand : List<AzureUploadFileCommand>, IRequest<IReadOnlyList<CloudFileResult>>
    {
    }
}
