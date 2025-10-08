using Common.Application.Notification.Abstractions;
using Common.Infrastructure.EmailService.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.EmailService.Handlers
{
    public class NotifyEmailCommandHandler : IRequestHandler<NotifyEmailCommand>
    {
        private readonly INotifyService _notifyService;
        public NotifyEmailCommandHandler(IServiceProvider serviceProvider)
        {
            _notifyService = serviceProvider.GetRequiredKeyedService<INotifyService>(nameof(NotifyEmailService));
        }
        public async Task Handle(NotifyEmailCommand request, CancellationToken cancellationToken)
        {
            await _notifyService.PublishNotification(request.Title, request.Message, request.Emails, request.Payload, cancellationToken);
        }
    }
}
