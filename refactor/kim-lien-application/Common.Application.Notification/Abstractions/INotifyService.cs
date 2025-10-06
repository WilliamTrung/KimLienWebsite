namespace Common.Application.Notification.Abstractions
{
    public interface INotifyService
    {
        public Task PublishNotification(string title, string message, List<string> userIds, object? payload = null, CancellationToken ct = default);
    }
}
