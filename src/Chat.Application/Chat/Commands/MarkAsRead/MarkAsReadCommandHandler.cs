using Chat.Application.Chat.Abstractions;
using MediatR;

namespace Chat.Application.Chat.Commands.MarkAsRead
{
    public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand>
    {
        private readonly IChatService _chatService;

        public MarkAsReadCommandHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            await _chatService.MarkAsReadAsync(request.RoomId, request.MessageIds, cancellationToken);
        }
    }
}


