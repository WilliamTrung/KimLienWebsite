using Authen.Application.Abstractions;
using Authen.Application.Commands;
using Authen.Application.Models;
using MediatR;

namespace Authen.Infrastructure.Handlers
{
    public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, TokenPair>
    {
        public async Task<TokenPair> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await authService.LoginAsync(request, cancellationToken);
        }
    }
}
