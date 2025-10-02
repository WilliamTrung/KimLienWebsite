using Common.Kernel.Constants;
using Common.Kernel.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using Common.DomainException.Abstractions;
using Microsoft.Extensions.Logging;

namespace Common.DomainException.Middleware
{
    public sealed class DomainExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<DomainExceptionMiddleware> _log;
        public DomainExceptionMiddleware(ILogger<DomainExceptionMiddleware> log) => _log = log;

        public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
        {
            try
            {
                // Let the rest of the pipeline run (controllers, etc.)
                await next(ctx);
            }
            catch (OperationCanceledException) when (ctx.RequestAborted.IsCancellationRequested)
            {
                await WriteProblem(ctx, 499, "Client closed request");
            }
            catch (CException ex)
            {
                _log.LogWarning(ex, "Domain exception");
                var payload = ex is IResultException rex
                    ? new ActionResponse<object?>
                    {
                        Data = rex.Result,
                        Message = ex.Message,
                        Code = ex.ErrorCode.GetValueOrDefault(),
                        StatusCode = ex.HttpStatusCode
                    }
                    : new ActionResponse
                    {
                        Message = ex.Message,
                        Code = ex.ErrorCode.GetValueOrDefault(),
                        StatusCode = ex.HttpStatusCode
                    };

                await WriteJson(ctx, (int)ex.HttpStatusCode, payload);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unhandled exception");
                var payload = new ActionResponse
                {
                    Message = "An unexpected error occurred.",
                    Code = (int)ResponseCode.System,
                    StatusCode = HttpStatusCode.InternalServerError
                };
                await WriteJson(ctx, StatusCodes.Status500InternalServerError, payload);
            }
        }

        private static async Task WriteProblem(HttpContext ctx, int status, string title)
        {
            if (ctx.Response.HasStarted) return;
            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/problem+json";
            await ctx.Response.WriteAsync($$"""{"title":"{{title}}","status":{{status}}}""");
        }

        private static async Task WriteJson(HttpContext ctx, int status, object payload)
        {
            if (ctx.Response.HasStarted) return;
            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(
                Newtonsoft.Json.JsonConvert.SerializeObject(payload,
                    new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.None }));
        }
    }

}
