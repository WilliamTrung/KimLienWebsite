using Microsoft.AspNetCore.HttpOverrides;

namespace Web.ApiHost.Extensions
{
    public static class ApplicationExtension
    {
        public static void ApplyApplicationExtensions(this WebApplication app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                // trust X-Forwarded-For, X-Forwarded-Proto, X-Real-IP
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,

                // Cloudflare adds only 1 proxy hop
                ForwardLimit = 2
            });

            // ❗ Required when behind proxies
            app.Use((context, next) =>
            {
                var userIP = context.Connection.RemoteIpAddress;
                Console.WriteLine("Client IP: " + userIP);
                return next();
            });
        }
        public static void ApplyCors(this IServiceCollection service)
        {
            service.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }
    }
}
