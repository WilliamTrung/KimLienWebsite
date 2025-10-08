using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureDeleteFileCommand : IRequest
    {
        public string FileDestination { get; set; } = null!;
    }
}
