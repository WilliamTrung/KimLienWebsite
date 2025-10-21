using Common.RequestContext.Abstractions;

namespace Common.RequestContext
{
    public class RequestContextData : IRequestContextData
    {
        public string? RequestId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserId { get; set; }
        public string? Email { get;set; }
        public string? UserAgent { get; set; }
        public List<string>? Roles { get; set; }
    }
}
