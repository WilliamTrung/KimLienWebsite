using ApiService.DTOs;
using AuthorizationLibrary.Models;
using JwtService;
using JwtService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtService.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfig;
        public JwtService(IOptions<JwtConfiguration> jwtConfig)
        {
            _jwtConfig= jwtConfig.Value;
        }

        public Task AddRolesClaim(ref ClaimsIdentity claims)
        {
            throw new Exception("Please add new class inherit from this class and override AddRolesClaim method! Then inject that class into project instead of this!");
        }
        
        public string GenerateAccessTokenAsync(User user)
        {
            Console.WriteLine("Issuer: " + _jwtConfig.Issuer);
            Console.WriteLine("Audience: " + _jwtConfig.Audience);
            Console.WriteLine("Key:" + _jwtConfig.Key);
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            var claims = new ClaimsIdentity();
            claims.AddClaim(claim: new Claim(JwtClaims.NameIdentifier, user.Id.ToString()));
            claims.AddClaim(claim: new Claim(ClaimTypes.Role, user.Role.Name));
            //await AddRolesClaim(ref claims);
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims.Claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtConfig.AccessTokenExpirationMinutes)),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateRefreshToken(),
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(_jwtConfig.RefreshTokenExpirationMinutes)
            };
            return refreshToken;

        }
        public string RefreshAccessTokenAsync(RefreshToken? refreshTokenEntity, User user)
        {
            //refreshTokenEntity is passed into by query from client's refreshtoken

            //var refreshTokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (refreshTokenEntity == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }
            if (refreshTokenEntity.UserId != user.Id)
            {
                throw new UnauthorizedAccessException("Invalid owner refresh token");
            }
            if (refreshTokenEntity.ExpirationDate <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Expired refresh token");
            }
            return GenerateAccessTokenAsync(user);

        }
        public ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtConfig.Issuer,
                    ValidAudience = _jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key))
                }, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }
    }
    
}
