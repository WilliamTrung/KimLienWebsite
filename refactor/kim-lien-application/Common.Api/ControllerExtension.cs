using Common.Kernel.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Common.Api
{
    public static class ControllerExtension
    {
        public static IActionResult CreateOk(this ControllerBase controller)
        {
            return controller.Ok(new ActionResponse() // Returns a 200 OK response with a success flag
            {
                IsSucceeded = true
            });
        }

        public static IActionResult CreateOkForResponse<T>(this ControllerBase controller, T result)
        {
            return controller.Ok(new ActionResponse<T>() // Returns a 200 OK response with the provided result
            {
                IsSucceeded = true,
                Data = result
            });
        }

        public static IActionResult CreateOkForListResponse<T>(this ControllerBase controller, List<T> result)
        {
            return controller.Ok(new ActionResponse<List<T>>() // Returns a 200 OK response with the provided result
            {
                IsSucceeded = true,
                Data = result
            });
        }

        public static IActionResult CreateBadRequest(this ControllerBase controller, string? errorMessage = null)
        {
            return controller.BadRequest(new ActionResponse() // Returns a 400 Bad Request response with an error message
            {
                IsSucceeded = false,
                Message = errorMessage
            });
        }

        public static IActionResult CreateBadRequestForResponse<T>(this ControllerBase controller, T result, string? errorMessage = null)
        {
            return controller.BadRequest(new ActionResponse<T>() // Returns a 400 Bad Request response with provided result and error message
            {
                IsSucceeded = false,
                Data = result,
                Message = errorMessage
            });
        }

        public static IActionResult CreateStatusCode<T>(this ControllerBase controller, HttpStatusCode httpStatusCode, T result, string? errorMessage = null)
        {
            ActionResponse? response = null; // Initialize response object

            // Check if the status code is OK
            if (HttpStatusCode.OK.Equals(httpStatusCode))
            {
                response = new ActionResponse<T>() // Create a success response with data
                {
                    Data = result,
                    IsSucceeded = true
                };
            }
            else // Create a failure response with data and error message
            {
                response = new ActionResponse<T>()
                {
                    IsSucceeded = false,
                    Data = result,
                    Message = errorMessage
                };
            }

            return controller.StatusCode((int)httpStatusCode, response); // Return the response with the specified status code
        }

        public static IActionResult CreateStatusCode(this ControllerBase controller, HttpStatusCode httpStatusCode, string errorMessage = null)
        {
            ActionResponse? response = null; // Initialize response object

            // Check if the status code is OK
            if (HttpStatusCode.OK.Equals(httpStatusCode))
            {
                response = new ActionResponse() // Create a success response
                {
                    IsSucceeded = true
                };
            }
            else // Create a failure response with an error message
            {
                response = new ActionResponse()
                {
                    IsSucceeded = false,
                    Message = errorMessage
                };
            }

            return controller.StatusCode((int)httpStatusCode, response); // Return the response with the specified status code
        }
    }
}
