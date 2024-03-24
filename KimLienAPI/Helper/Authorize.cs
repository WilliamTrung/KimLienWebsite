using KL_Service.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? authHeader = context.HttpContext.Request.Headers["Authorization"];
            SetConfiguration(context);
            TokenModel? authorized = null;
            string message = "Unauthorized";
            try
            {
                authorized = _authService.ValidateToken(authHeader).Result;
                bool roleAuthorized = true;
                if (_roles.Count > 0 && !_roles.Any(r => r == authorized.Role))
                {
                    //role authorizing
                    roleAuthorized = false;
                    message += " - Unauthorized for this function!";
                }
                isAuthorized = roleAuthorized;
                if (!isAuthorized)
                {
                    if (_roles.Count > 0)
                    {
                        message += " - Required Role(s): " + GetAlertRequiredRoles();
                        if (authorized != null)
                        {
                            message += " - Current role: " + authorized.Role.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                isAuthorized = false;
            }
            if (!isAuthorized)
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
