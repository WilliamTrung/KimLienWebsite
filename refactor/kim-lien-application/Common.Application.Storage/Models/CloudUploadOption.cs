namespace Common.Application.Storage.Models
{
    public sealed class CloudUploadOption
    {
        public string DestinationKey { get; init; } = null!;
        public string ContentType { get; init; } = "application/octet-stream";
        public IDictionary<string, string>? Metadata { get; init; }
        public bool PublicRead { get; init; } = false;
    }
}
