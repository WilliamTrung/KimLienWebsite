using Authen.Application.Abstractions;
using Authen.Application.Models;
using Common.Domain.Entities;
using Common.Extension.Jwt;
using Common.Kernel.Dependencies;
using Common.RequestContext.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Authen.Infrastructure.Services
{
    public sealed class AuthService(
        UserManager<User> users,
        IRefreshTokenService refreshSvc,
        IRequestContext context,
        IOptions<JwtSettings> jwtSetting)
        : IAuthService, IScoped
    {
        private readonly JwtSettings jwt = jwtSetting.Value;
        public async Task RegisterAsync(RegisterDto dto, CancellationToken ct)
        {
            var user = new User { 
                UserName = dto.Email,
                Email = dto.Email,
                DisplayName = dto.DisplayName ?? dto.Email, 
                PhoneNumber = dto.PhoneNumber,
                Region = dto.Region 
            };
            var result = await users.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task<TokenPair> LoginAsync(LoginDto dto, CancellationToken ct)
        {
            var ip = context.Data.IpAddress;
            var ua = context.Data.UserAgent;
            var user = await users.FindByEmailAsync(dto.Email);
            if (user is null || !await users.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException("invalid_credentials");

            var roles = await users.GetRolesAsync(user);
            var (access, exp) = JwtExtension.IssueAccess(user, roles, jwt);

            var (refresh, _) = await refreshSvc.CreateAsync(user, familyId: null, ip, ua, jwt, ct);

            return new TokenPair { AccessToken = access, ExpiresAtUtc = exp, RefreshToken = refresh };
        }

        public async Task<TokenPair> RefreshAsync(RefreshDto dto, CancellationToken ct)
        {
            var ip = context.Data.IpAddress;
            var ua = context.Data.UserAgent;
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
