using Common.Application.Storage.Models;
using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureUploadFileCommand : FileUpload, IRequest<CloudFileResult>
    {
    }
}
