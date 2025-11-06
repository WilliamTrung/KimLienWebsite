using Chat.Application.Chat.Abstractions;
using Chat.Infrastructure.Abstractions;
using Chat.Infrastructure.DataHub;
using MediatR;
using Microsoft.AspNet.SignalR;
namespace Chat.Application.Chat.Commands.SendMessage
{
    public class SendMessageCommandHandler(IChatService chatService, IHubContext<ChatHub> chatHub, IConnectionPoolProvider connectionPoolProvider) : IRequestHandler<SendMessageCommand>
    {
        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await chatService.SendMessage(new Models.MessageDto
            {
                RoomId = request.RoomId,
                Message = request.Content,
                Metadata = request.Payload,
            });
            var userIds = await chatService.GetUserInRoom(request.RoomId);
            var userIdString = userIds.Select(x => x.ToString()).ToList();
            var connections = connectionPoolProvider.GetConnection(userIdString);
            chatHub.Clients.Clients(connections).Se(new
            {
                RoomId = request.RoomId,
                Message = request.Content,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
