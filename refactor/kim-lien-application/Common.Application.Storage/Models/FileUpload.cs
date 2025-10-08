namespace Common.Application.Storage.Models
{
    public class FileUpload
    {
        // Choose ONE of Content / Bytes / LocalPath. Prefer Stream via UploadStreamAsync for big files.
        public byte[]? Bytes { get; init; }
        public string? LocalPath { get; init; }     // if you allow server-side file path uploads
        public string FileName { get; init; } = null!;
        public string ContentType { get; init; } = "application/octet-stream";
        public string DestinationKey { get; init; } = null!; // e.g. "products/123/photo.jpg"
        public IDictionary<string, string>? Metadata { get; init; }
        public bool PublicRead { get; init; } = false;       // expose via public URL?
        public IDictionary<string, string>? Tags { get; init; }
    }
}
