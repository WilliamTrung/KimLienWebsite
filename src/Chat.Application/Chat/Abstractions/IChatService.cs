using Chat.Application.Chat.Models;
using Common.Kernel.Response.Pagination;

namespace Chat.Application.Chat.Abstractions
{
    public interface IChatService
    {
        /// <summary>
        /// Send a message via SignalR Hub (recommended method following methodology)
        /// </summary>
        Task<MessageDto> SendMessageAsync(string senderId, Guid conversationId, string content, object? metadata = null);
        
        /// <summary>
        /// Legacy method for backward compatibility
        /// </summary>
        Task SendMessage(MessageDto messageDto);
        
        /// <summary>
        /// Get room IDs for a user
        /// </summary>
        Task<List<Guid>> GetRoomIdsByUser(string userId);
        
        /// <summary>
        /// Get chat history for a room (REST API - recommended for history)
        /// </summary>
        Task<PaginationResponse<MessageDto>> GetChatHistoryAsync(string roomId, int page, int pageSize, CancellationToken cancellationToken);
        
        /// <summary>
        /// Mark messages as read (REST API - recommended for read receipts)
        /// </summary>
        Task MarkAsReadAsync(string roomId, List<string>? messageIds, CancellationToken cancellationToken);
        
        /// <summary>
        /// Leave a room - updates database record status
        /// </summary>
        Task LeaveRoomAsync(string userId, string roomId, CancellationToken cancellationToken);
    }
}
