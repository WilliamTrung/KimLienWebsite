using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureDeleteBatchCommand : IRequest
    {
        public List<string> FileDestinations { get; set; } = new();
    }
}
