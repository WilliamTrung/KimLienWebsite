using MediatR;

namespace Chat.Application.Chat.Commands.MarkAsRead
{
    public class MarkAsReadCommand : IRequest
    {
        public string RoomId { get; set; } = null!;
        public List<string>? MessageIds { get; set; } // If null, mark all messages in room as read
    }
}


