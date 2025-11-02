namespace Chat.Application.Chat.Abstractions
{
    public interface IChatService
    {
        Task SendMessage(string userId, string content, object? payload = null);
        Task OnConnected(string userId, string connectionId);
    }
}
