using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Xml;

namespace Common.DomainException.Middleware
{
    public class DomainExceptionMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext handler, RequestDelegate next)
        {
            if (((int)HttpStatusCode.InternalServerError).Equals(handler.Response.StatusCode))
            {
                handler.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json; // Set response content type to JSON
                var contextFeature = handler.Features.Get<IExceptionHandlerFeature>(); // Get the exception feature
                var httpStatusCode = (int)HttpStatusCode.BadRequest; // Default to BadRequest status code
                object response = null; // Initialize response object

                // Handle specific domain exceptions
                if (contextFeature.Error is DomainException domainException)
                {
                    if (domainException is IResultException resultException)
                    {
                        response = new FailActionResponse<object>() // Create a response for a result exception
                        {
                            Data = resultException.Result,
                            ErrorMessage = domainException.Message,
                            ErrorCode = domainException.ErrorCode.GetValueOrDefault(),
                        };
                    }
                    else
                    {
                        response = new FailActionResponse() // Create a response for a general domain exception
                        {
                            ErrorMessage = domainException.Message,
                            ErrorCode = domainException.ErrorCode.GetValueOrDefault(),
                        };
                    }
                }
                else if (contextFeature.Error is not null) // Handle non-domain exceptions
                {
                    if (contextFeature.Error is IResultException resultException)
                    {
                        response = resultException.Result; // Use result from the result exception
                        httpStatusCode = (int)HttpStatusCode.OK; // Set status code to OK for result exceptions
                    }
                    else
                    {
                        response = new FailActionResponse() // Create a response for general exceptions
                        {
                            ErrorCode = ErrorCode.System,
                            ErrorMessage = contextFeature.Error.Message
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
                        ContractResolver = _contractResolver // Use the contract resolver for camel case property names
                    }));
                }
            }
        }
    }
}
