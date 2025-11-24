using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureDeleteFileCommand : IRequest, IAzureCommand
    {
        public string ProfileKey { get; set; } = null!;
        public string FileDestination { get; set; } = null!;
    }
}
