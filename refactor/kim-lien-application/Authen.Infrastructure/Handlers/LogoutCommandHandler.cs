using Authen.Application.Abstractions;
using Authen.Application.Commands;
using MediatR;

namespace Authen.Infrastructure.Handlers
{
    public class LogoutCommandHandler(IAuthService authService) : IRequestHandler<LogoutCommand>
    {
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await authService.LogoutAsync(request, cancellationToken);
        }
    }
}
