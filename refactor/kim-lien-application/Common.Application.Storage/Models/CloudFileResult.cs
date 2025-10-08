namespace Common.Application.Storage.Models
{
    public sealed class CloudFileResult
    {
        public string Key { get; init; } = null!;   // provider object key
        public string Url { get; init; } = null!;   // public or signed (depending on ACL)
        public long? SizeBytes { get; init; }
        public string? ETag { get; init; }          // version/hash if provider supports
    }
}
