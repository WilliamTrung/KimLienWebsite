using KL_Service.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OData.UriParser;
using Models.Enum;
using Models.ServiceModels.Auth;
using System.Net;

namespace KimLienAPI.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class Authorize : ActionFilterAttribute, IActionFilter
    {
        private readonly Role[]? _roles;
        private IAuthService _authService = null!;
        public Authorize(params string[]? roles)
        {
            if (roles != null)
            {
                _roles = roles.Select(str => (Models.Enum.Role)Enum.Parse(typeof(Models.Enum.Role), str)).ToArray();
            }
        }
        private void SetConfiguration(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            _authService = (IAuthService)serviceProvider.GetRequiredService(typeof(IAuthService));
        }
        private string? GetToken(string? token)
        {
            if (token == null)
            {
                return null;
            }
            token = token.Split(" ")[1];
            if (token == null || token.Length == 0)
            {
                return null;
            }
            return token;
            //var claims = DeserializedToken(token);
            //var email = claims.First(c => c.Type == CustomClaimTypes.Email).Value;
            //var user = await _unitOfWork.UserRepository.GetFirst(c => c.Email == email);
            //return _mapper.Map<TokenModel>(user);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? authHeader = context.HttpContext.Request.Headers["Authorization"];
            SetConfiguration(context);
            bool authorized;
            string message = "Unauthorized";
            try
            {
                var token = GetToken(authHeader);
                if(token == null)
                {
                    throw new InvalidOperationException();
                }
                var claims = _authService.DeserializeToken(token);
                if (claims == null)
                {
                    throw new InvalidOperationException();
                }
                authorized = _authService.ValidateToken(claims.ToArray(), _roles);
            }
            catch (Exception)
            {
                authorized = false;
            }
            if (!authorized)
            {

                context.Result = context.Result = new ObjectResult(message)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }
        }
    }
}
