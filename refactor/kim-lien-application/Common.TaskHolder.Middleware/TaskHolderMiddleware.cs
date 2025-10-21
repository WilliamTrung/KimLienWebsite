using Common.TaskHolder.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Common.TaskHolder.Middleware
{
    public class TaskHolderMiddleware(ITaskHolder taskHolder) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next.Invoke(context);
            taskHolder.WaitAll();
        }
    }
}
