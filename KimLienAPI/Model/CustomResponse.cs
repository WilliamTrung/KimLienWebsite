namespace KimLienAPI.Model
{
    public class CustomResponse
    {
        public CustomResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Accepted",
                201 => "Created",
                202 => "Accepted",
                409 => "Conflict",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden action",
                404 => "No resource found",
                500 => "Server error",
                _ => "Unexpected error has occured"
            };
        }
    }
}
