using Chat.Application.Chat.Commands.GetRoomsByUserQuery;
using Chat.Infrastructure.Implementations;
using Chat.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Infrastructure.DataHub
{
    public class ChatHub(ISender sender, ConnectionPoolProvider connectionPool) : Hub
    {
        private ClaimsPrincipal ContextUser => (ClaimsPrincipal)Context.User;
        private CurrentUser? _currUser { get; set; }
        private CurrentUser CurrentUser
        {
            get
            {
                if (_currUser == null)
                {
                    _currUser = new()
                    {
                        UserId = Context.UserIdentifier,
                        ConnectionId = Context.ConnectionId
                    };
                }
                return _currUser;
            }
        }
        public override async Task OnConnectedAsync()
        {
            connectionPool.AddConnection(CurrentUser.UserId, CurrentUser.ConnectionId);
            await AddUserRoomGroup();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            connectionPool.RemoveConnection(CurrentUser.UserId);
            return base.OnDisconnectedAsync(exception);
        }
        // Typing indicator
        public async Task Typing(string roomId)
        {
            await Clients.OthersInGroup(roomId).SendAsync("UserTyping", new
            {
                RoomId = roomId,
                CurrentUser.UserId,
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task StopTyping(string roomId)
        {
            await Clients.OthersInGroup(roomId)
                .SendAsync("UserStoppedTyping", new
                {
                    RoomId = roomId,
                    CurrentUser.UserId,
                    Timestamp = DateTime.UtcNow
                });
        }
        private async Task AddUserRoomGroup()
        {
            var roomIds = await sender.Send(new GetRoomsByUserQuery
            {
                UserId = Guid.Parse(CurrentUser.UserId),
            });
            if (roomIds is not null)
            {
                foreach (var roomId in roomIds)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                } 
            }
        }
    }
}
