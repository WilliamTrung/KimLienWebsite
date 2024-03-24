using KL_AuthFeature;
using Models.ServiceModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KL_Service.AuthService
{
    public interface IAuthService
    {
        string GetToken(LoginRequestModel loginRequest);
        IEnumerable<Claim>? DeserializeToken(string token);        
    }
    public class AuthService : IAuthService
    {
        private readonly IAuthFeature _authFeature;
        public AuthService(IAuthFeature authFeature)
        {
            _authFeature = authFeature;
        }
        public IEnumerable<Claim>? DeserializeToken(string token)
        {
            return _authFeature.DeserializedToken(token);
        }

        public string GetToken(LoginRequestModel loginRequest)
        {
            return _authFeature.Login(loginRequest);
        }
    }
}
