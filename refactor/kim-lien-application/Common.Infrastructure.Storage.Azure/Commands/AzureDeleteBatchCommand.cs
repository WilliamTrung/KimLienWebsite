using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureDeleteBatchCommand : IRequest, IAzureCommand
    {
        public string ProfileKey { get; set; } = null!;
        public List<string> FileDestinations { get; set; } = new();
    }
}
