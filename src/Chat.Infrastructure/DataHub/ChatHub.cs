using Chat.Application.Chat.Abstractions;
using Chat.Application.Chat.Commands.GetRoomsByUserQuery;
using Chat.Application.Chat.Models;
using Chat.Infrastructure.Implementations;
using Chat.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Infrastructure.DataHub
{
    /// <summary>
    /// SignalR Hub for real-time chat communication.
    /// Follows methodology: Hub delegates business logic to ChatService.
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly ISender _sender;
        private readonly ConnectionPoolProvider _connectionPool;
        private readonly IChatService _chatService;
        private CurrentUser? _currUser;

        public ChatHub(
            ISender sender,
            ConnectionPoolProvider connectionPool,
            IChatService chatService)
        {
            _sender = sender;
            _connectionPool = connectionPool;
            _chatService = chatService;
        }
        
        private CurrentUser CurrentUser
        {
            get
            {
                if (_currUser == null)
                {
                    var userId = Context.UserIdentifier 
                        ?? throw new UnauthorizedAccessException("User is not authenticated");
                    
                    _currUser = new()
                    {
                        UserId = userId,
                        ConnectionId = Context.ConnectionId
                    };
                }
                return _currUser;
            }
        }

        public override async Task OnConnectedAsync()
        {
            // Add connection to pool for presence tracking
            _connectionPool.AddConnection(CurrentUser.UserId, CurrentUser.ConnectionId);
            
            // Add user to their room groups
            await AddUserRoomGroup();
            
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _connectionPool.RemoveConnection(CurrentUser.UserId);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Send a message via SignalR Hub (REQUIRED method per methodology).
        /// 
        /// Flow:
        /// 1. Client sends message via SignalR
        /// 2. Hub delegates to ChatService for business logic and persistence
        /// 3. Hub broadcasts message to conversation group (real-time notification)
        /// 
        /// This is the ONLY way to send messages with real-time notifications.
        /// </summary>
        public async Task SendMessage(SendMessageDto dto)
        {
            // Never trust senderId from client - always use Context.UserIdentifier
            var userId = Context.UserIdentifier 
                ?? throw new UnauthorizedAccessException("User is not authenticated");

            if (!Guid.TryParse(dto.RoomId, out var roomId))
            {
                await Clients.Caller.SendAsync("Error", new { Message = "Invalid RoomId" });
                return;
            }

            try
            {
                // Step 1: Delegate to application service (following methodology)
                // ChatService handles: authorization, validation, persistence
                var message = await _chatService.SendMessageAsync(
                    userId,
                    roomId,
                    dto.Content,
                    dto.Payload
                );

                // Step 2: Broadcast to conversation group (excluding sender's connections)
                // This provides real-time notification to all participants
                var senderConnections = _connectionPool.GetConnection(userId) ?? new List<string>();
                
                // Notify all participants in the room (except sender)
                await Clients.GroupExcept(dto.RoomId, senderConnections)
                    .SendAsync("ReceiveMessage", message);

                // Step 3: Send confirmation to sender
                await Clients.Caller.SendAsync("MessageSent", message);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Typing indicator - real-time only, no persistence needed
        /// </summary>
        public async Task Typing(string roomId)
        {
            await Clients.OthersInGroup(roomId).SendAsync("UserTyping", new
            {
                RoomId = roomId,
                UserId = CurrentUser.UserId,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Stop typing indicator
        /// </summary>
        public async Task StopTyping(string roomId)
        {
            await Clients.OthersInGroup(roomId)
                .SendAsync("UserStoppedTyping", new
                {
                    RoomId = roomId,
                    UserId = CurrentUser.UserId,
                    Timestamp = DateTime.UtcNow
                });
        }

        /// <summary>
        /// Join a room group for receiving messages
        /// </summary>
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserJoined", new
            {
                RoomId = roomId,
                UserId = CurrentUser.UserId,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Leave a room - updates database via ChatService and removes from SignalR group
        /// </summary>
        public async Task LeaveRoom(string roomId)
        {
            if (!Guid.TryParse(roomId, out var roomGuid))
            {
                await Clients.Caller.SendAsync("Error", new { Message = "Invalid RoomId" });
                return;
            }

            try
            {
                // Step 1: Update database via ChatService
                await _chatService.LeaveRoomAsync(CurrentUser.UserId, roomId, CancellationToken.None);

                // Step 2: Remove from SignalR group
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

                // Step 3: Notify other participants
                await Clients.Group(roomId).SendAsync("UserLeft", new
                {
                    RoomId = roomId,
                    UserId = CurrentUser.UserId,
                    Timestamp = DateTime.UtcNow
                });

                // Step 4: Confirm to caller
                await Clients.Caller.SendAsync("RoomLeft", new
                {
                    RoomId = roomId,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", new { Message = ex.Message });
            }
        }

        private async Task AddUserRoomGroup()
        {
            var roomIds = await _sender.Send(new GetRoomsByUserQuery
            {
                UserId = Guid.Parse(CurrentUser.UserId),
            });
            
            if (roomIds is not null && roomIds.Count > 0)
            {
                foreach (var roomId in roomIds)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                }
            }
        }
    }

    /// <summary>
    /// DTO for sending messages via SignalR Hub
    /// </summary>
    public class SendMessageDto
    {
        public string RoomId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public object? Payload { get; set; }
    }
}
