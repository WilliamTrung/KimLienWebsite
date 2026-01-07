using Chat.Application.Chat.Abstractions;
using Chat.Application.Chat.Models;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Chat.Application.Chat.Commands.GetChatHistory
{
    public class GetChatHistoryQueryHandler : IRequestHandler<GetChatHistoryQuery, PaginationResponse<MessageDto>>
    {
        private readonly IChatService _chatService;

        public GetChatHistoryQueryHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<PaginationResponse<MessageDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _chatService.GetChatHistoryAsync(
                request.RoomId,
                request.Page,
                request.PageSize,
                cancellationToken);
        }
    }
}

