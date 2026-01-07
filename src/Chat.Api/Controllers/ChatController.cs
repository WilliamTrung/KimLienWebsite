using Chat.Application.Chat.Commands.GetChatHistory;
using Chat.Application.Chat.Commands.GetRoomsByUserQuery;
using Chat.Application.Chat.Commands.MarkAsRead;
using Common.Api;
using Common.Kernel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    /// <summary>
    /// REST API controller for chat QUERY operations only.
    /// 
    /// All real-time operations (send message, leave room, typing, etc.) MUST use SignalR Hub.
    /// This controller only provides query endpoints for data retrieval.
    /// </summary>
    [ApiController]
    [Authorize(Roles = Roles.Default)]
    [Route("/api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly ISender _sender;

        public ChatController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Load chat history for a room (Query only)
        /// </summary>
        [HttpGet("rooms/{roomId}/messages")]
        public async Task<IActionResult> GetChatHistory(
            [FromRoute] string roomId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken ct = default)
        {
            var query = new GetChatHistoryQuery
            {
                RoomId = roomId,
                Page = page,
                PageSize = pageSize
            };

            var result = await _sender.Send(query, ct);
            return this.CreateOk(result);
        }

        /// <summary>
        /// Get all rooms for the current user (Query only)
        /// </summary>
        [HttpGet("rooms")]
        public async Task<IActionResult> GetUserRooms(CancellationToken ct)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User is not authenticated");

            var query = new GetRoomsByUserQuery
            {
                UserId = Guid.Parse(userId)
            };

            var result = await _sender.Send(query, ct);
            return this.CreateOk(result);
        }

        /// <summary>
        /// Mark messages as read (Query/State management only)
        /// </summary>
        [HttpPost("messages/mark-read")]
        public async Task<IActionResult> MarkAsRead([FromBody] MarkAsReadCommand command, CancellationToken ct)
        {
            await _sender.Send(command, ct);
            return this.CreateOk();
        }
    }
}
