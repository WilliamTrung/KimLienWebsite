using MediatR;

namespace Common.Infrastructure.Storage.Azure.Commands
{
    public class AzureDeleteByTagCommand : IRequest<int>
    {
        public Dictionary<string, string> Tags { get; set; } = new();
        public bool MatchAny { get; set; } = false; // if true, delete if ANY tag matches; if false, delete only if ALL tags match
    }
}
