using AutoMapper;
using Chat.Application.Chat.Abstractions;
using Chat.Application.Chat.Models;
using Chat.Application.Common.Abstractions;
using Chat.Application.Common.Models;
using Chat.Infrastructure.Data;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.Extension;
using Common.Kernel.Response.Pagination;
using Common.RequestContext.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Common.Extension.Logging;
using Newtonsoft.Json;
namespace Chat.Infrastructure.Services
{
    /// <summary>
    /// Chat service following methodology:
    /// - Handles business logic and persistence
    /// - Does NOT handle real-time broadcasting (that's Hub's responsibility)
    /// - Returns DTOs for Hub to broadcast
    /// </summary>
    public class ChatService(IRequestContext requestContext
        , ChatContext chatContext
        , IMapper mapper
        , ILogger<ChatService> logger)
        : IChatService
    {
        public async Task<MessageDto> SendMessageAsync(string senderId, Guid conversationId, string content, object? metadata = null)
        {
            // 1. Authorization check - verify user exists
            if (!Guid.TryParse(senderId, out var userId))
            {
                logger.LogDataInformation($"Invalid user id: {senderId}");
                throw new CException("Invalid user id", System.Net.HttpStatusCode.BadRequest);
            }

            var user = await chatContext.Users
                .Where(x => x.Id == userId)
                .Include(x => x.UserMetadata)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                logger.LogDataInformation($"User not found for id: {userId}");
                throw new CException("User not found", System.Net.HttpStatusCode.NotFound);
            }

            // 2. Verify room exists and user has access
            var room = await chatContext.ChatRooms
                .FirstOrDefaultAsync(r => r.Id == conversationId);

            if (room is null)
            {
                logger.LogDataInformation($"Room not found: {conversationId}");
                throw new CException("Room not found", System.Net.HttpStatusCode.NotFound);
            }

            // 3. Create Message entity
            var message = new ChatMessage
            {
                RoomId = conversationId,
                SenderId = user.Id,
                Message = content,
                Metadata = metadata?.ToDocument(),
                IpAddress = requestContext.Data.IpAddress ?? throw new CException("IpAddress is null", System.Net.HttpStatusCode.BadRequest),
                SentAt = DateTime.UtcNow
            };

            // 4. Save to database
            chatContext.Add(message);
            await chatContext.SaveChangesAsync();

            // 5. Return DTO
            var userDto = mapper.Map<UserDto>(user);
            var messageDto = new MessageDto
            {
                Id = message.Id.ToString(),
                RoomId = conversationId.ToString(),
                Message = content,
                SenderId = senderId,
                Sender = userDto,
                SentAt = message.SentAt,
                Metadata = metadata
            };

            return messageDto;
        }

        /// <summary>
        /// Legacy method - DO NOT USE. Messages should be sent via SignalR Hub.
        /// This method only persists the message but does NOT broadcast.
        /// Broadcasting must be done in ChatHub.
        /// </summary>
        public async Task SendMessage(MessageDto messageDto)
        {
            // Only persist - broadcasting is handled by ChatHub
            await SendMessageAsync(
                messageDto.SenderId ?? throw new CException("SenderId is required", System.Net.HttpStatusCode.BadRequest),
                Guid.Parse(messageDto.RoomId),
                messageDto.Message,
                messageDto.Metadata);
            
            // NOTE: Broadcasting is NOT done here - it must be done in ChatHub
            // This follows the methodology: Hub handles real-time delivery
        }
        public async Task<List<Guid>> GetRoomIdsByUser(string userId)
        {
            if (Guid.TryParse(userId, out var userGuid))
            {
                var sessions = await chatContext.ChatSessions.Where(x => x.UserId.HasValue && x.UserId == userGuid && !x.IsDeleted).ToListAsync();
                return sessions.Select(x => x.RoomId).ToList();
            }
            return new List<Guid>();
        }

        public async Task<PaginationResponse<MessageDto>> GetChatHistoryAsync(string roomId, int page, int pageSize, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(roomId, out var roomGuid))
            {
                throw new ArgumentException("Invalid RoomId", nameof(roomId));
            }

            var query = chatContext.ChatMessages
                .Where(m => m.RoomId == roomGuid)
                .Include(m => m.Sender)
                    .ThenInclude(s => s!.UserMetadata)
                .OrderByDescending(m => m.SentAt);

            var totalCount = await query.CountAsync(cancellationToken);
            
            var messages = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(m => m.SentAt) // Return in chronological order
                .ToListAsync(cancellationToken);

            var messageDtos = messages.Select(m => new MessageDto
            {
                Id = m.Id.ToString(),
                RoomId = roomId,
                Message = m.Message,
                SenderId = m.SenderId?.ToString(),
                Sender = m.Sender != null ? mapper.Map<UserDto>(m.Sender) : null,
                SentAt = m.SentAt,
                Metadata = m.Metadata?.TryDeserializeObject<object>(),
            }).ToList();

            return new PaginationResponse<MessageDto>
            {
                Results = messageDtos,
                CurrentPage = page - 1, // PaginationResponse uses 0-based indexing
                PageSize = pageSize,
                RowCount = totalCount
            };
        }

        public async Task MarkAsReadAsync(string roomId, List<string>? messageIds, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(requestContext.Data.UserId!);
            
            if (!Guid.TryParse(roomId, out var roomGuid))
            {
                throw new ArgumentException("Invalid RoomId", nameof(roomId));
            }

            // TODO: Implement read receipt tracking
            // This would typically involve:
            // 1. Creating a ReadReceipt entity
            // 2. Tracking which messages have been read by which users
            // 3. Updating read status
            
            // For now, this is a placeholder implementation
            await Task.CompletedTask;
            
            logger.LogDataInformation($"Marked messages as read for user {userId} in room {roomId}");
        }

        public async Task LeaveRoomAsync(string userId, string roomId, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(userId, out var userGuid))
            {
                throw new ArgumentException("Invalid UserId", nameof(userId));
            }

            if (!Guid.TryParse(roomId, out var roomGuid))
            {
                throw new ArgumentException("Invalid RoomId", nameof(roomId));
            }

            // Find the chat session for this user and room
            var session = await chatContext.ChatSessions
                .FirstOrDefaultAsync(s => s.UserId == userGuid && s.RoomId == roomGuid, cancellationToken);

            if (session != null)
            {
                // Update session status - mark as inactive or delete
                // Soft delete by interceptor
                chatContext.ChatSessions.Remove(session);
                await chatContext.SaveChangesAsync(cancellationToken);
                
                logger.LogDataInformation($"User {userId} left room {roomId} - session removed");
            }
            else
            {
                logger.LogDataInformation($"User {userId} was not in room {roomId}");
            }
        }
    }
}
