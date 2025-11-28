using Common.Kernel.Models.Implementations;
using MediatR;

namespace Chat.Application.Chat.Commands.SendMessage
{
    public class SendMessageCommand : IRequest
    {
        public string RoomId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public List<AssetDto>? Payload { get; set; }
    }
}
