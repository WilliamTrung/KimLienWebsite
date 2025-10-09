using Common.Application.Storage.Models;
using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureUploadFileCommand : FileUpload, IRequest<CloudFileResult>, IAzureCommand
    {
        public string ProfileKey { get; set; } = null!;
    }
}
