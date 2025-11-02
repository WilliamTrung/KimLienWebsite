using Chat.Application.Common.Abstractions;
using Chat.Application.Common.Models;
using MediatR;
using Microsoft.AspNet.SignalR;
using System.Security.Claims;

namespace Chat.Api.Hubs
{
    public class ChatHub(ISender sender, IConnectionPoolProvider connectionPool) : Hub
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
                        UserId = ContextUser.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? throw new InvalidOperationException("User is not authenticated."),
                        ConnectionId = Context.ConnectionId
                    };
                }
                return _currUser;
            }
        }
        public override Task OnConnected()
        {
            connectionPool.AddConnection(CurrentUser.UserId, CurrentUser.ConnectionId);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            connectionPool.RemoveConnection(CurrentUser.UserId);
            return base.OnDisconnected(stopCalled);
        }

        // Typing indicator
        public async Task Typing(string roomId)
        {
            await Clients.Group(roomId, Context.ConnectionId)
                .SendAsync("UserTyping", new
                {
                    RoomId = roomId,
                    UserId = CurrentUser.UserId,
                    Timestamp = DateTime.UtcNow
                });
        }

        public async Task StopTyping(string roomId)
        {
            await Clients.Group(roomId, Context.ConnectionId)
                .SendAsync("UserStoppedTyping", new
                {
                    RoomId = roomId,
                    UserId = CurrentUser.UserId,
                    Timestamp = DateTime.UtcNow
                });
        }
    }
}
