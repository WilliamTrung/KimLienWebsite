using Chat.Application.Chat.Models;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Chat.Application.Chat.Commands.GetChatHistory
{
    public class GetChatHistoryQuery : IRequest<PaginationResponse<MessageDto>>
    {
        public string RoomId { get; set; } = null!;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
