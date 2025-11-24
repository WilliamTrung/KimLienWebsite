using Chat.Application.Chat.Abstractions;
using MediatR;

namespace Chat.Application.Chat.Commands.GetRoomsByUserQuery
{
    public class GetRoomsByUserQueryHandler(IChatService chatService) : IRequestHandler<GetRoomsByUserQuery, List<Guid>>
    {
        public async Task<List<Guid>> Handle(GetRoomsByUserQuery request, CancellationToken cancellationToken)
        {
            return await chatService.GetRoomIdsByUser(request.UserId.ToString());
        }
    }
}
