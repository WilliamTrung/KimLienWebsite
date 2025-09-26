using Common.Kernel.Constants;
using Common.Kernel.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Common.DomainException.Middleware
{
    public class DomainExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext handler, RequestDelegate next)
        {
            object? response = null;
            int httpStatusCode = (int)HttpStatusCode.OK;
            if (((int)HttpStatusCode.InternalServerError).Equals(handler.Response.StatusCode))
            {
                handler.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json; // Set response content type to JSON
                var contextFeature = handler.Features.Get<IExceptionHandlerFeature>(); // Get the exception feature
                httpStatusCode = (int)HttpStatusCode.BadRequest; // Default to BadRequest status code

                // Handle specific domain exceptions
                if (contextFeature.Error is DomainException domainException)
                {
                    if (domainException is IResultException resultException)
                    {
                        response = new ActionResponse<object>() // Create a response for a result exception
                        {
                            Data = resultException.Result,
                            Message = domainException.Message,
                            Code = domainException.ErrorCode.GetValueOrDefault(),
                            StatusCode = domainException.HttpStatusCode,
                        };
                    }
                    else
                    {
                        response = new ActionResponse() // Create a response for a general domain exception
                        {
                            Message = domainException.Message,
                            Code = domainException.ErrorCode.GetValueOrDefault(),
                            StatusCode = domainException.HttpStatusCode,
                        };
                    }
                }
                else if (contextFeature.Error is not null) // Handle non-domain exceptions
                {
                    response = new ActionResponse() // Create a response for general exceptions
                    {
                        Message = contextFeature.Error.Message,
                        Code = (int)ResponseCode.System,
                        StatusCode = HttpStatusCode.InternalServerError,
                    };
                }
            }
            // If a response was created, set the status code and write the response to the body
            if (response is not null)
            {
                handler.Response.StatusCode = httpStatusCode; // Set the HTTP status code
                await handler.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    Formatting = Formatting.None, // Minimize formatting for the JSON
                }));
            }
        }
    }
}
