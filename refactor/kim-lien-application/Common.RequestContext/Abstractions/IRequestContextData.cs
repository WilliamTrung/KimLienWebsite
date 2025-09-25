namespace Common.RequestContext.Abstractions
{
    public interface IRequestContextData
    {
        string? IpAddress { get; set; }
        string? UserId { get; set; }
        string? Email { get; set; }
        string? RequestId { get; set; }
    }
}
