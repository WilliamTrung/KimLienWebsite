using AutoMapper;
using KL_Repository.UnitOfWork;
using KL_Validation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Config.AuthConfig;
using Models.Enum;
using Models.ServiceModels.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KL_AuthFeature
{
    public interface IAuthFeature
    {
        string Login(LoginRequestModel loginUser);
        //Task<TokenModel?> ValidateToken(string? token);
        IEnumerable<Claim>? DeserializedToken(string accessToken);
        string GenerateToken(TokenModel user);
    }
    public class AuthFeature : IAuthFeature
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtConfig _jwtConfig;
        public AuthFeature(IUnitOfWork unitOfWork, IOptions<JwtConfig> jwtConfig)
        {
            _unitOfWork = unitOfWork;
            _jwtConfig = jwtConfig.Value;
        }
        public IEnumerable<Claim>? DeserializedToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;
            if(jsonToken == null)
            {
                return null;
            }
            var idClaim = jsonToken.Claims.First(c => c.Type == CustomClaims.ID);
            if (idClaim != null)
            {
                var decryptedIdClaim = new Claim(CustomClaims.ID, AesOperation.Decrypt(idClaim.Value, _jwtConfig.Key));
                jsonToken.Claims.Append(decryptedIdClaim);
            }
            return jsonToken.Claims;
        }
        public string GenerateToken(TokenModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(CustomClaims.ID, AesOperation.Encrypt(user.Id.ToString(), _jwtConfig.Key)),
                new Claim(CustomClaims.Role,user.Role.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),                
            };
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private TokenModel GetUser(LoginRequestModel loginRequest)
        {
            var user = _unitOfWork.UserRepository.GetFirst(c => c.Id == loginRequest.Id);
            if(user == null)
            {
                throw new KeyNotFoundException("No account found for id: " + loginRequest.Id);
            }
            if(user.Password == loginRequest.Password)
            {
                var tokenModel = new TokenModel()
                {
                    Id = user.Id,
                    Role = user.Role
                };
                return tokenModel;
            } else
            {
                throw new InvalidOperationException("Wrong password");
            }
        }
        public string Login(LoginRequestModel loginUser)
        {
            var tokenModel = GetUser(loginUser);
            var token = GenerateToken(tokenModel);
            return token;
        }

        //public Task<TokenModel?> ValidateToken(string? token)
        //{
        //    if (token == null)
        //    {
        //        return null;
        //    }
        //    token = token.Split(" ")[1];
        //    if (token == null || token.Length == 0)
        //    {
        //        return null;
        //    }
        //    var claims = DeserializedToken(token);
        //    var email = claims.First(c => c.Type == CustomClaims.Email).Value;
        //    var user = _unitOfWork.UserRepository.GetFirst(c => c.Email == email);
        //    return _mapper.Map<TokenModel>(user);
        //}
    }
}