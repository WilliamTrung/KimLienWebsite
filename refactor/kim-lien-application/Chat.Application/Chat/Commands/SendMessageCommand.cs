using MediatR;

namespace Chat.Application.Chat.Commands
{
    public class SendMessageCommand : IRequest
    {
        public string UserId { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
