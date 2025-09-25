using Common.Extension.Logging;
using Common.RequestContext.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace Common.Logging.Middleware
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly IRequestContext _requestContext;
        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger, IRequestContext requestContext)
        {
            _logger = logger;
            _requestContext = requestContext;
        }
        // Method that will be called for each HTTP request
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Start measuring the time taken for the request
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Enable buffering to read the request body multiple times
            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBody(context.Request); // Read the request body

            // Log request details
            _logger.LogDataInformation($"RequestId Start: {_requestContext.Data.RequestId} " +
                $"| Uri: {GetUri(context.Request)} " +
                $"| Token: {(context.Request.Headers.TryGetValue("authorization", out var token) ? token : string.Empty)} " +
                $"| Request: {requestBody} ");

            // Store the original response body stream
            var originalBodyStream = context.Response.Body;

            try
            {
                using (var responseBodyStream = new MemoryStream())
                {
                    context.Response.Body = responseBodyStream; // Use the memory stream as the response body
                    await next.Invoke(context); // Call the next middleware in the pipeline

                    stopwatch.Stop(); // Stop the stopwatch

                    //// Read the response body from the memory stream
                    string? responseBodyText = null;
                    if (context.Response.Body.CanSeek)
                    {
                        context.Response.Body.Seek(0, SeekOrigin.Begin); // Rewind the response stream
                        responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync(); // Read the response body
                        context.Response.Body.Seek(0, SeekOrigin.Begin); // Rewind the response stream again 
                    }

                    // Log response details
                    _logger.LogDataInformation($"RequestId End: {_requestContext.Data.RequestId} " +
                        $"| Uri: {GetUri(context.Request)} " +
                        $"| StatusCode:{context.Response.StatusCode} " +
                        $"| Response: {responseBodyText} " +
                        $"| Duration: {stopwatch.ElapsedMilliseconds}");

                    // Copy the response body back to the original stream
                    await responseBodyStream.CopyToAsync(originalBodyStream);
                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }


        // Helper method to read the request body
        static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.Body.Position = 0; // Ensure the position is at the beginning
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true); // Use a StreamReader to read the request body
            var body = await reader.ReadToEndAsync(); // Read the body content
            request.Body.Position = 0; // Reset the position for further processing
            return body; // Return the body content as a string
        }
        static Uri GetUri(HttpRequest request)
        {
            var builder = new UriBuilder();
            builder.Scheme = request.Scheme;
            builder.Host = request.Host.Value;
            builder.Path = request.Path;
            builder.Query = request.QueryString.ToUriComponent();
            return builder.Uri;
        }
    }
}
