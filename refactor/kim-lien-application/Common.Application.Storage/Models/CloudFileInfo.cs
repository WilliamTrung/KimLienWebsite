namespace Common.Application.Storage.Models
{
    public sealed class CloudFileInfo
    {
        public string Key { get; init; } = null!;
        public string Url { get; init; } = null!;
        public long SizeBytes { get; init; }
        public DateTimeOffset LastModifiedUtc { get; init; }
        public string ContentType { get; init; } = "application/octet-stream";
        public IDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
    }
}
