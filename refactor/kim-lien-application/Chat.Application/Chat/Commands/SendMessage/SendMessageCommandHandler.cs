using Chat.Application.Chat.Abstractions;
using MediatR;
namespace Chat.Application.Chat.Commands.SendMessage
{
    public class SendMessageCommandHandler(IChatService chatService) : IRequestHandler<SendMessageCommand>
    {
        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await chatService.SendMessage(new Models.MessageDto
            {
                RoomId = request.RoomId,
                Message = request.Content,
                Metadata = request.Payload,
            });
        }
    }
}
