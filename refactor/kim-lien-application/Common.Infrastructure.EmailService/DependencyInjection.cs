using Common.Application.Notification.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.EmailService
{
    public static class DependencyInjection
    {
        public static void RegisterEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
            services.AddKeyedTransient<INotifyService, NotifyEmailService>(nameof(NotifyEmailService));
        }
    }
}
