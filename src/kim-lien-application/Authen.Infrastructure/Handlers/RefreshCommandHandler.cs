using Authen.Application.Abstractions;
using Authen.Application.Commands;
using Authen.Application.Models;
using MediatR;

namespace Authen.Infrastructure.Handlers
{
    public class RefreshCommandHandler(IAuthService authService) : IRequestHandler<RefreshCommand, TokenPair>
    {
        public async Task<TokenPair> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            return await authService.RefreshAsync(request, cancellationToken);
        }
    }
}
