using Chat.Application.Chat.Abstractions;
using Common.RequestContext.Abstractions;
using MediatR;

namespace Chat.Application.Chat.Commands.SendMessage
{
    /// <summary>
    /// ⚠️ DEPRECATED: Handler for sending messages via REST API
    /// 
    /// This handler is deprecated. Messages MUST be sent via SignalR Hub for real-time delivery.
    /// 
    /// This handler only persists the message but does NOT send real-time notifications.
    /// Real-time notifications are ONLY available via SignalR Hub.
    /// 
    /// Use SignalR Hub method: hubConnection.invoke("SendMessage", { roomId, content, payload })
    /// </summary>
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>
    {
        private readonly IChatService _chatService;
        private readonly IRequestContext _requestContext;

        public SendMessageCommandHandler(IChatService chatService, IRequestContext requestContext)
        {
            _chatService = chatService;
            _requestContext = requestContext;
        }

        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var userId = _requestContext.Data.UserId 
                ?? throw new UnauthorizedAccessException("User is not authenticated");

            if (!Guid.TryParse(request.RoomId, out var roomId))
            {
                throw new ArgumentException("Invalid RoomId", nameof(request));
            }

            // This only persists - NO real-time notification
            // Clients will NOT receive ReceiveMessage event
            await _chatService.SendMessageAsync(userId, roomId, request.Content, request.Payload);
        }
    }
}
