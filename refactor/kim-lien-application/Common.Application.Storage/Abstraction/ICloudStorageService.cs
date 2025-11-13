using Common.Application.Storage.Models;

namespace Common.Application.Storage.Abstraction
{
    public interface ICloudStorageService
    {
        //  Upload
        Task<CloudFileResult> UploadAsync(FileUpload file, CancellationToken ct = default);
        Task<IReadOnlyList<CloudFileResult>> UploadAsync(IEnumerable<FileUpload> files, CancellationToken ct = default);

        // Stream-first (large files, no buffering the whole byte[])
        Task<CloudFileResult> UploadStreamAsync(Stream content, CloudUploadOption options, CancellationToken ct = default);

        //  Delete
        Task DeleteAsync(string fileUrlOrKey, CancellationToken ct = default);
        Task DeleteManyAsync(IEnumerable<string> fileUrlsOrKeys, CancellationToken ct = default);

        //  Move / Copy 
        Task<CloudFileResult> MoveAsync(string sourceUrlOrKey, string destinationKey, CancellationToken ct = default);
        Task<CloudFileResult> CopyAsync(string sourceUrlOrKey, string destinationKey, CancellationToken ct = default);

        //  Info / Utility 
        Task<bool> ExistsAsync(string fileUrlOrKey, CancellationToken ct = default);
        Task<CloudFileInfo?> GetInfoAsync(string fileUrlOrKey, CancellationToken ct = default);

        // Time-limited, signed URL for direct client download (or upload if needed)
        Task<string> GetDownloadUrlAsync(string fileUrlOrKey, TimeSpan ttl, CancellationToken ct = default);

        // Optional: list by prefix/folder
        Task<IReadOnlyList<CloudFileInfo>> ListAsync(string prefix, int? limit = null, CancellationToken ct = default);
        // ── Tags (generic) ────────────────────────────────────────────────────────
        Task<IDictionary<string, string>> GetTagsAsync(string fileUrlOrKey, CancellationToken ct = default);
        Task RemoveTagsAsync(string fileUrlOrKey, IEnumerable<string> tagKeys, CancellationToken ct = default);
        /// <summary>
        /// Deletes blobs that match the given tags.
        /// If <paramref name="matchAny"/> is false (default), all provided tag pairs must match (AND).
        /// If true, any matching tag pair will be deleted (OR).
        /// Returns number of blobs deleted.
        /// </summary>
        Task<int> DeleteByTagsAsync(
            IReadOnlyDictionary<string, string> tags,
            bool matchAny = false,
            CancellationToken ct = default);
    }
}
