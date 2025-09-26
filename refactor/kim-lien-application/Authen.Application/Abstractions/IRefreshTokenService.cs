using Authen.Domain.Entities;
using Common.Domain.Entities;
using Common.Extension.Jwt;

namespace Authen.Application.Abstractions
{
    public interface IRefreshTokenService
    {
        Task<(string token, RefreshToken saved)> CreateAsync(User user, Guid? familyId, string? ip, string? ua, JwtSettings s, CancellationToken ct);
        Task<(User user, RefreshToken current)> ValidateAsync(string providedToken, CancellationToken ct);
        Task<RefreshToken> RotateAsync(RefreshToken current, string? ip, string? ua, JwtSettings s, CancellationToken ct);
        Task RevokeFamilyAsync(Guid familyId, Guid userId, CancellationToken ct);
    }
}
