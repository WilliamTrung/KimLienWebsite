using Authen.Application.Abstractions;
using Authen.Domain.Entities;
using Authen.Infrastructure.Data;
using Common.Domain.Entities;
using Common.Extension.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Authen.Infrastructure.Services
{
    public sealed class RefreshTokenService(AuthenIdentityDbContext db) : IRefreshTokenService
    {
        static string Hash(string token)
        {
            var bytes = SHA256.HashData(Convert.FromBase64String(token));
            return Convert.ToBase64String(bytes);
        }
        static string NewOpaque() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public async Task<(string token, RefreshToken saved)> CreateAsync(User user, Guid? familyId, string? ip, string? ua, JwtSettings s, CancellationToken ct)
        {
            var token = NewOpaque();
            var saved = new RefreshToken
            {
                UserId = user.Id,
                FamilyId = familyId ?? Guid.NewGuid(),
                TokenHash = Hash(token),
                ExpiresUtc = DateTime.UtcNow.AddDays(s.RefreshDays),
                CreatedByIp = ip,
                UserAgent = ua
            };
            db.RefreshTokens.Add(saved);
            await db.SaveChangesAsync(ct);
            return (token, saved);
        }

        public async Task<(User user, RefreshToken current)> ValidateAsync(string providedToken, CancellationToken ct)
        {
            var tokenHash = Hash(providedToken);
            var rt = await db.RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash, ct);

            if (rt is null || rt.RevokedUtc != null || rt.ExpiresUtc < DateTime.UtcNow)
                throw new UnauthorizedAccessException("invalid_refresh");

            if (rt.ConsumedUtc != null)
            {
                // reuse attempt → revoke whole family
                await RevokeFamilyAsync(rt.FamilyId, rt.UserId, ct);
                throw new UnauthorizedAccessException("reused_refresh");
            }

            return (rt.User, rt);
        }

        public async Task<RefreshToken> RotateAsync(RefreshToken current, string? ip, string? ua, JwtSettings s, CancellationToken ct)
        {
            current.ConsumedUtc = DateTime.UtcNow;

            var nextToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var next = new RefreshToken
            {
                UserId = current.UserId,
                FamilyId = current.FamilyId,
                TokenHash = Hash(nextToken),
                ExpiresUtc = DateTime.UtcNow.AddDays(s.RefreshDays),
                CreatedByIp = ip,
                UserAgent = ua
            };
            db.RefreshTokens.Add(next);

            await db.SaveChangesAsync(ct);

            // return the entity with the *plain* token attached via Tag (not persisted)
            // simplest: stash it in CurrentValues[“_plain”] or just return entity and token separately at callsite
            return next; // caller keeps the plain token string separately
        }

        public async Task RevokeFamilyAsync(Guid familyId, Guid userId, CancellationToken ct)
        {
            await db.RefreshTokens
                .Where(r => r.UserId == userId && r.FamilyId == familyId && r.RevokedUtc == null)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.RevokedUtc, DateTime.UtcNow), ct);
        }
    }
}
