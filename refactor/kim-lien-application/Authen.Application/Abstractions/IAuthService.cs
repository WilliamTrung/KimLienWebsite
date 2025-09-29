using Authen.Application.Models;

namespace Authen.Application.Abstractions
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto, CancellationToken ct);
        Task<TokenPair> LoginAsync(LoginDto dto, CancellationToken ct);
        Task<TokenPair> RefreshAsync(RefreshDto dto, CancellationToken ct);
        Task LogoutAsync(RefreshDto dto, CancellationToken ct);
    }
}
