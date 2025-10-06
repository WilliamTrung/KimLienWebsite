using Common.Application.Notification.Abstractions;

namespace Common.Infrastructure.EmailService
{
    public class NotifyEmailService : INotifyService
    {
        public Task PublishNotification(string title, string message, List<string> userIds, object? payload = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
