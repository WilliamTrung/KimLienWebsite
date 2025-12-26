using Authen.Application.Abstractions;
using Authen.Application.Commands;
using MediatR;

namespace Authen.Application.Handlers
{
    public class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommand>
    {
        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await authService.RegisterAsync(request, cancellationToken);
        }
    }
}
