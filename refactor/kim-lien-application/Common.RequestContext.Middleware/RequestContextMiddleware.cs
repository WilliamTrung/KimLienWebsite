using System.Security.Claims;
using Common.RequestContext.Abstractions;
using Microsoft.AspNetCore.Http;

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
                UserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = user?.FindFirst(ClaimTypes.Email)?.Value,
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
            // Cloudflare header first
            if (http.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfIp))
                return cfIp.ToString();

            // then X-Forwarded-For
            if (http.Request.Headers.TryGetValue("X-Forwarded-For", out var xff))
                return xff.ToString().Split(',')[0];

            // fallback
            return http.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}
