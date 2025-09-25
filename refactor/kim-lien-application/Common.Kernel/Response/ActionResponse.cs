using System.Net;

namespace Common.Kernel.Response
{
    public class ActionResponse
    {
        public bool IsSucceeded { get; set; }
        public string Content { get; set; } = null!;
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public int Code { get; set; }
    }
    public class ActionResponse<T> : ActionResponse
    {
        public T Data { get; set; } = default!;
    }
}
