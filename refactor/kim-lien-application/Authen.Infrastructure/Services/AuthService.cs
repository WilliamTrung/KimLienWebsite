using Authen.Application.Abstractions;
using Authen.Application.Models;
using Common.Domain.Entities;
using Common.Extension.Jwt;
using Microsoft.AspNetCore.Identity;

namespace Authen.Infrastructure.Services
{
    public sealed class AuthService(
        UserManager<User> users,
        IRefreshTokenService refreshSvc,
        JwtSettings jwt)
        : IAuthService
    {
        public async Task RegisterAsync(RegisterDto dto, CancellationToken ct)
        {
            var user = new User { UserName = dto.Email, Email = dto.Email, DisplayName = dto.DisplayName };
            var result = await users.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task<TokenPair> LoginAsync(LoginDto dto, string? ip, string? ua, CancellationToken ct)
        {
            var user = await users.FindByEmailAsync(dto.Email);
            if (user is null || !await users.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException("invalid_credentials");

            var roles = await users.GetRolesAsync(user);
            var (access, exp) = JwtExtension.IssueAccess(user, roles, jwt);

            var (refresh, _) = await refreshSvc.CreateAsync(user, familyId: null, ip, ua, jwt, ct);

            return new TokenPair { AccessToken = access, ExpiresAtUtc = exp, RefreshToken = refresh };
        }

        public async Task<TokenPair> RefreshAsync(RefreshDto dto, string? ip, string? ua, CancellationToken ct)
        {
            var (user, current) = await refreshSvc.ValidateAsync(dto.RefreshToken, ct);

            // rotate by creating the next token in same family
            var (newRefresh, _) = await refreshSvc.CreateAsync(user, current.FamilyId, ip, ua, jwt, ct);

            current.ConsumedUtc = DateTime.UtcNow; // mark used
                                                   // If your RefreshTokenService.RotateAsync handles both, call that instead.

            var roles = await users.GetRolesAsync(user);
            var (access, exp) = JwtExtension.IssueAccess(user, roles, jwt);

            return new TokenPair { AccessToken = access, ExpiresAtUtc = exp, RefreshToken = newRefresh };
        }

        public async Task LogoutAsync(RefreshDto dto, CancellationToken ct)
        {
            var (user, current) = await refreshSvc.ValidateAsync(dto.RefreshToken, ct);
            await refreshSvc.RevokeFamilyAsync(current.FamilyId, user.Id, ct);
        }
    }
}
