using MediatR;

namespace Common.Infrastructure.EmailService.Commands
{
    public class NotifyEmailCommand : IRequest
    {
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public List<string> Emails { get; set; }
        public object? Payload { get; set; }
    }
}
