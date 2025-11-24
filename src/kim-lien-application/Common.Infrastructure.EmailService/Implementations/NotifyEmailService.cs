using Common.Application.Notification.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Common.Infrastructure.EmailService
{
    public class NotifyEmailService : INotifyService
    {
        private readonly SmtpOptions _opt;
        private readonly ILogger<NotifyEmailService> _log;

        public NotifyEmailService(IOptions<SmtpOptions> opt, ILogger<NotifyEmailService> log)
        {
            _opt = opt.Value;
            _log = log;
        }

        public async Task PublishNotification(
            string title,
            string message,
            List<string> userIds,
            object? payload = null,
            CancellationToken ct = default)
        {
            if (userIds is null || userIds.Count == 0) return;

            using var client = new SmtpClient(_opt.Host, _opt.Port)
            {
                EnableSsl = _opt.EnableSsl,
                Credentials = new NetworkCredential(_opt.User, _opt.Password)
            };
            var mail = new MailMessage
            {
                From = new MailAddress(_opt.FromEmail, _opt.FromName),
                Subject = title,
                Body = message ?? string.Empty,
                IsBodyHtml = false // plain text, as requested
            };
            foreach (var raw in userIds)
            {
                ct.ThrowIfCancellationRequested();

                var to = (raw ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(to)) continue;

                mail.To.Add(to);
            }
            var credentials = string.Join(", ", userIds.Select(x => x));
            try
            {
                await client.SendMailAsync(mail, ct);
                _log.LogInformation("Sent email to {Email}", credentials);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to send email to {Email}", credentials);
                // decide whether to rethrow or continue
                throw;
            }
        }
    }
}
