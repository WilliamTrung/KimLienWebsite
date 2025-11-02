using Chat.Application.Chat.Abstractions;
using Chat.Application.Chat.Commands;
using MediatR;

namespace Chat.Application.Chat.Handlers
{
    public class SendMessageCommandHandler(IChatService chatService) : IRequestHandler<SendMessageCommand>
    {
        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await chatService.SendMessage(request.UserId, request.Content);
        }
    }
}
