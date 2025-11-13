using MediatR;

namespace Chat.Application.Chat.Commands.GetRoomsByUserQuery
{
    public class GetRoomsByUserQuery : IRequest<List<Guid>>
    {
        public Guid UserId { get; set; }
    }
}
