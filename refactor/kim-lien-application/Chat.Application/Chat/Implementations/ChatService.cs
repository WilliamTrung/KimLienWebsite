using Chat.Application.Chat.Abstractions;

namespace Chat.Application.Chat.Implementations
{
    public class ChatService : IChatService
    {
        public Task OnConnected(string userId, string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task SendMessage(string userId, string content, object? payload)
        {
            throw new NotImplementedException();
        }
    }
}
