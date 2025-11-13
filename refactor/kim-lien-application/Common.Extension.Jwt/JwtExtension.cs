using Common.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Common.Extension.Jwt
{
    public static class JwtExtension
    {
        public static (string token, DateTime expiresUtc) IssueAccess(
            User user, IEnumerable<string> roles, JwtSettings s)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(s.AccessMinutes);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var jwt = new JwtSecurityToken(s.Issuer, s.Audience, claims, now, expires, creds);
            return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
        }
    }
}
