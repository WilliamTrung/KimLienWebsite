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
        SignInManager<User> signInMgr,
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
            if (user is null)
                throw new UnauthorizedAccessException("invalid_credentials");

            // Uses lockout policy configured in Identity options
            var result = await signInMgr.CheckPasswordSignInAsync(
                user, dto.Password, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var roles = await users.GetRolesAsync(user);
                var (access, exp) = JwtExtension.IssueAccess(user, roles, jwt);
                var (refresh, _) = await refreshSvc.CreateAsync(user, familyId: null, ip, ua, jwt, ct);

                // For cookie auth you could call: await signInMgr.SignInAsync(user, isPersistent:false);
                return new TokenPair { AccessToken = access, ExpiresAtUtc = exp, RefreshToken = refresh };
            }

            if (result.IsLockedOut) throw new UnauthorizedAccessException("user_locked_out");
            if (result.RequiresTwoFactor) throw new UnauthorizedAccessException("requires_2fa");
            if (result.IsNotAllowed) throw new UnauthorizedAccessException("not_allowed");

            throw new UnauthorizedAccessException("invalid_credentials");
        }

        public async Task<TokenPair> RefreshAsync(RefreshDto dto, CancellationToken ct)
        {
            var ip = context.Data.IpAddress;
            var ua = context.Data.UserAgent;

            var (user, current) = await refreshSvc.ValidateAsync(dto.RefreshToken, ct);

            // rotate within same family
            var (newRefresh, _) = await refreshSvc.CreateAsync(user, current.FamilyId, ip, ua, jwt, ct);
            current.ConsumedUtc = DateTime.UtcNow;

            // optional: validate security stamp to ensure access token is not issued for revoked users
            // if (!await signInMgr.CanSignInAsync(user)) throw new UnauthorizedAccessException("not_allowed");

            var roles = await users.GetRolesAsync(user);
            var (access, exp) = JwtExtension.IssueAccess(user, roles, jwt);

            return new TokenPair { AccessToken = access, ExpiresAtUtc = exp, RefreshToken = newRefresh };
        }

        public async Task LogoutAsync(RefreshDto dto, CancellationToken ct)
        {
            var (user, current) = await refreshSvc.ValidateAsync(dto.RefreshToken, ct);

            // Revoke the entire refresh-token family
            await refreshSvc.RevokeFamilyAsync(current.FamilyId, user.Id, ct);

            // If you also use cookie auth anywhere:
            await signInMgr.SignOutAsync();
            // Optionally invalidate all access via security stamp:
            await users.UpdateSecurityStampAsync(user);
        }
    }
}
