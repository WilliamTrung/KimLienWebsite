using Common.Application.Notification.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.EmailService
{
    public static class DependencyInjection
    {
        public static void RegisterEmailService(this IServiceCollection services, IConfiguration configuration, string sectionName = "SmtpOptions")
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.Configure<SmtpOptions>(configuration.GetSection(sectionName));
            services.AddKeyedTransient<INotifyService, NotifyEmailService>(nameof(NotifyEmailService));
        }
    }
}
