using Common.RequestContext.Abstractions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Common.RequestContext.Middleware
{
    public class RequestContextMiddleware : IMiddleware
    {
        private readonly IRequestContext _requestContext;
        public RequestContextMiddleware(IRequestContext requestContext)
        {
            _requestContext = requestContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Authentication should already have run: app.UseAuthentication()
            var user = context.User;
            var data = new RequestContextData
            {
                UserId = user?.FindFirst(JwtRegisteredClaimNames.NameId)?.Value,
                Email = user?.FindFirst(JwtRegisteredClaimNames.Email)?.Value,
                IpAddress = GetClientIp(context),
                RequestId = context.Request.Headers["request-id"],
                UserAgent = context.Request.Headers["user-agent"],
                Roles = user?.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList()
            };
            _requestContext.Set(data);                             // store in scoped holder
            await next.Invoke(context);
        }
        private static string? GetClientIp(HttpContext http)
        {
            var ip = http.Connection.RemoteIpAddress;
            if (ip is null) return null;
            if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();
            return ip.ToString();
        }
    }
}
