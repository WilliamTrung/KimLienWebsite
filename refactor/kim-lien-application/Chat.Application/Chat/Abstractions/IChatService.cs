using Chat.Application.Chat.Models;

namespace Chat.Application.Chat.Abstractions
{
    public interface IChatService
    {
        Task SendMessage(MessageDto messageDto);
        Task<List<Guid>> GetRoomIdsByUser(string userId);
    }
}
