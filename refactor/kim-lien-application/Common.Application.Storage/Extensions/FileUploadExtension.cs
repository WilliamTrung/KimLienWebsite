using Common.Application.Storage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
namespace Common.Application.Storage.Extensions
{

    public static class FileUploadExtension
    {
        /// <summary>
        /// Convert IFormFile to FileUpload. Small files => Bytes; large files => LocalPath.
        /// </summary>
        /// <param name="file">Incoming form file</param>
        /// <param name="destinationPrefix">e.g. "products/123" (no trailing slash needed)</param>
        /// <param name="publicRead">Whether the uploaded object should be publicly readable</param>
        /// <param name="metadata">Optional user metadata</param>
        /// <param name="tags">Optional object tags</param>
        /// <param name="inMemoryThresholdBytes">Files larger than this are written to disk</param>
        public static async Task<FileUpload> ToFileUploadAsync(
            this IFormFile file,
            string destinationPrefix,
            bool publicRead = false,
            IDictionary<string, string>? metadata = null,
            IDictionary<string, string>? tags = null,
            long inMemoryThresholdBytes = 5 * 1024 * 1024, // 5 MB default,
            bool autoGenerateFileName = false,
            CancellationToken cancellationToken = default)
        {
            if (file is null) throw new ArgumentNullException(nameof(file));
            if (string.IsNullOrWhiteSpace(destinationPrefix)) throw new ArgumentException("Destination prefix is required.", nameof(destinationPrefix));

            var originalName = autoGenerateFileName ? Guid.NewGuid().ToString() : Path.GetFileName(file.FileName ?? "file");
            var safeName = SanitizeFileName(originalName);
            var destinationKey = $"{destinationPrefix.TrimEnd('/')}/{safeName}";

            // Content type (fallback by extension, then octet-stream)
            var contentType = !string.IsNullOrWhiteSpace(file.ContentType) ? file.ContentType : InferContentType(originalName);

            if (file.Length <= inMemoryThresholdBytes)
            {
                // Buffer into memory
                await using var ms = new MemoryStream((int)Math.Max(file.Length, 0));
                await file.CopyToAsync(ms, cancellationToken);
                return new FileUpload
                {
                    Bytes = ms.ToArray(),
                    LocalPath = null,
                    FileName = originalName,
                    ContentType = contentType,
                    DestinationKey = destinationKey,
                    Metadata = metadata,
                    PublicRead = publicRead,
                    Tags = tags
                };
            }
            else
            {
                // Spill to disk (temp) and return LocalPath to avoid large allocations
                var tempDir = Path.Combine(Path.GetTempPath(), "upload-spool");
                Directory.CreateDirectory(tempDir);
                var tempPath = Path.Combine(tempDir, $"{Guid.NewGuid():N}-{safeName}");

                await using (var fs = new FileStream(tempPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 64 * 1024, useAsync: true))
                {
                    await file.CopyToAsync(fs, cancellationToken);
                }

                return new FileUpload
                {
                    Bytes = null,
                    LocalPath = tempPath,
                    FileName = originalName,
                    ContentType = contentType,
                    DestinationKey = destinationKey,
                    Metadata = metadata,
                    PublicRead = publicRead,
                    Tags = tags
                };
            }
        }

        private static string SanitizeFileName(string name)
        {
            // keep extension, strip path, replace spaces, remove risky chars
            var baseName = Path.GetFileName(name);
            foreach (var c in Path.GetInvalidFileNameChars())
                baseName = baseName.Replace(c, '_');
            return baseName.Replace(' ', '-');
        }

        private static string InferContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            return provider.TryGetContentType(fileName, out var ct)
                ? ct
                : "application/octet-stream";
        }
    }
}
