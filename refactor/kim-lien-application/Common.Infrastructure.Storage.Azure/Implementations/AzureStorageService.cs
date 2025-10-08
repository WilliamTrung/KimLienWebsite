using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Common.Application.Storage.Abstraction;
using Common.Application.Storage.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Infrastructure.Storage.Azure.Implementations
{
    public sealed class AzureStorageService : ICloudStorageService
    {
        private readonly BlobServiceClient _svc;
        private readonly AzureBlobOption _opt;
        private readonly ILogger<AzureStorageService> _log;

        public AzureStorageService(
            BlobServiceClient serviceClient,
            IOptions<AzureBlobOption> opt,
            ILogger<AzureStorageService> log)
        {
            _svc = serviceClient;
            _opt = opt.Value;
            _log = log;
        }

        // Ensure container exists (lazy)
        private async Task<BlobContainerClient> GetContainerAsync(CancellationToken ct)
        {
            var container = _svc.GetBlobContainerClient(_opt.Container);
            await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
            return container;
        }

        private static string KeyFromUrl(Uri uri)
        {
            // assumes url like: https://{account}.blob.core.windows.net/{container}/{key}
            // returns "{key}"
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 2) throw new ArgumentException("URL does not contain container/key path", nameof(uri));
            // segments[0] = container, rest = key parts
            return string.Join('/', segments.Skip(1));
        }

        private async Task<BlobClient> GetBlobClientAsync(string urlOrKey, CancellationToken ct)
        {
            var container = await GetContainerAsync(ct);
            if (Uri.TryCreate(urlOrKey, UriKind.Absolute, out var uri))
            {
                var key = KeyFromUrl(uri);
                return container.GetBlobClient(key);
            }
            return container.GetBlobClient(urlOrKey);
        }

        public async Task<CloudFileResult> UploadAsync(FileUpload file, CancellationToken ct = default)
        {
            var container = await GetContainerAsync(ct);
            var blob = container.GetBlobClient(file.DestinationKey);

            // Resolve content stream
            Stream stream = file.Bytes is not null
                ? new MemoryStream(file.Bytes, writable: false)
                : (file.LocalPath is not null ? File.OpenRead(file.LocalPath) : throw new ArgumentException("Provide Bytes or LocalPath"));

            await using (stream)
            {
                var headers = new BlobHttpHeaders { ContentType = file.ContentType };
                var meta = file.Metadata ?? new Dictionary<string, string>();
                var tags = file.Tags ?? new Dictionary<string, string>();

                var resp = await blob.UploadAsync(stream, new BlobUploadOptions
                {
                    Tags = tags,
                    HttpHeaders = headers,
                    Metadata = meta,
                    TransferOptions = new StorageTransferOptions { MaximumConcurrency = 4 }
                }, ct);

                return new CloudFileResult
                {
                    Key = file.DestinationKey,
                    Url = blob.Uri.ToString(),
                    SizeBytes = stream.CanSeek ? stream.Length : null,
                    ETag = resp.Value.ETag.ToString()
                };
            }
        }

        public async Task<IReadOnlyList<CloudFileResult>> UploadAsync(IEnumerable<FileUpload> files, CancellationToken ct = default)
        {
            var list = new List<CloudFileResult>();
            foreach (var f in files)
            {
                ct.ThrowIfCancellationRequested();
                list.Add(await UploadAsync(f, ct));
            }
            return list;
        }

        public async Task<CloudFileResult> UploadStreamAsync(Stream content, CloudUploadOption options, CancellationToken ct = default)
        {
            var container = await GetContainerAsync(ct);
            var blob = container.GetBlobClient(options.DestinationKey);

            var headers = new BlobHttpHeaders { ContentType = options.ContentType };
            var meta = options.Metadata ?? new Dictionary<string, string>();
            var tags = options.Tags ?? new Dictionary<string, string>();

            var resp = await blob.UploadAsync(content, new BlobUploadOptions
            {
                Tags = tags,
                HttpHeaders = headers,
                Metadata = meta
            }, ct);

            return new CloudFileResult
            {
                Key = options.DestinationKey,
                Url = blob.Uri.ToString(),
                SizeBytes = content.CanSeek ? content.Length : null,
                ETag = resp.Value.ETag.ToString()
            };
        }

        public async Task DeleteAsync(string fileUrlOrKey, CancellationToken ct = default)
        {
            var blob = await GetBlobClientAsync(fileUrlOrKey, ct);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: ct);
        }

        public async Task DeleteManyAsync(IEnumerable<string> fileUrlsOrKeys, CancellationToken ct = default)
        {
            foreach (var f in fileUrlsOrKeys)
            {
                ct.ThrowIfCancellationRequested();
                await DeleteAsync(f, ct);
            }
        }

        public async Task<CloudFileResult> MoveAsync(string sourceUrlOrKey, string destinationKey, CancellationToken ct = default)
        {
            var res = await CopyAsync(sourceUrlOrKey, destinationKey, ct);
            await DeleteAsync(sourceUrlOrKey, ct);
            return res;
        }

        public async Task<CloudFileResult> CopyAsync(string sourceUrlOrKey, string destinationKey, CancellationToken ct = default)
        {
            var container = await GetContainerAsync(ct);
            var dest = container.GetBlobClient(destinationKey);

            // Build a source URI; if private, create a short-lived SAS for read
            var src = await GetBlobClientAsync(sourceUrlOrKey, ct);
            Uri sourceUri = src.Uri;

            // If you need to ensure SAS for private reads:
            if (!_opt.ContainerIsPublic)
            {
                var sas = src.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(10));
                sourceUri = sas;
            }
            var srcProps = await src.GetPropertiesAsync();
            var srcTags = (await src.GetTagsAsync()).Value.Tags; // IDictionary<string,string>
            var copyOptions = new BlobCopyFromUriOptions
            {
                Metadata = new Dictionary<string, string>(srcProps.Value.Metadata),
                Tags = new Dictionary<string, string>(srcTags)
            };
            var op = await dest.StartCopyFromUriAsync(sourceUri, copyOptions, cancellationToken: ct);
            // Optionally wait until completed:
            var props = await dest.GetPropertiesAsync(cancellationToken: ct);

            return new CloudFileResult
            {
                Key = destinationKey,
                Url = dest.Uri.ToString(),
                SizeBytes = props.Value.ContentLength,
                ETag = props.Value.ETag.ToString()
            };
        }

        public async Task<bool> ExistsAsync(string fileUrlOrKey, CancellationToken ct = default)
        {
            var blob = await GetBlobClientAsync(fileUrlOrKey, ct);
            var exists = await blob.ExistsAsync(ct);
            return exists.Value;
        }

        public async Task<CloudFileInfo?> GetInfoAsync(string fileUrlOrKey, CancellationToken ct = default)
        {
            var blob = await GetBlobClientAsync(fileUrlOrKey, ct);
            try
            {
                var props = await blob.GetPropertiesAsync(cancellationToken: ct);
                return new CloudFileInfo
                {
                    Key = KeyFromUrl(blob.Uri),
                    Url = blob.Uri.ToString(),
                    SizeBytes = props.Value.ContentLength,
                    LastModifiedUtc = props.Value.LastModified,
                    ContentType = props.Value.ContentType ?? "application/octet-stream",
                    Metadata = new Dictionary<string, string>(props.Value.Metadata)
                };
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<string> GetDownloadUrlAsync(string fileUrlOrKey, TimeSpan ttl, CancellationToken ct = default)
        {
            var blob = await GetBlobClientAsync(fileUrlOrKey, ct);

            // If the container is public, direct URL is fine
            if (_opt.ContainerIsPublic)
                return blob.Uri.ToString();

            // Otherwise, return a SAS URL
            var expires = DateTimeOffset.UtcNow.Add(ttl);
            var sas = blob.GenerateSasUri(BlobSasPermissions.Read, expires);
            return sas.ToString();
        }

        public async Task<IReadOnlyList<CloudFileInfo>> ListAsync(string prefix, int? limit = null, CancellationToken ct = default)
        {
            var container = await GetContainerAsync(ct);
            var list = new List<CloudFileInfo>();
            await foreach (var item in container.GetBlobsAsync(traits: BlobTraits.Metadata, states: BlobStates.None, prefix: prefix, cancellationToken: ct))
            {
                var blob = container.GetBlobClient(item.Name);
                list.Add(new CloudFileInfo
                {
                    Key = item.Name,
                    Url = blob.Uri.ToString(),
                    SizeBytes = item.Properties.ContentLength ?? 0,
                    LastModifiedUtc = item.Properties.LastModified ?? DateTimeOffset.MinValue,
                    ContentType = item.Properties.ContentType ?? "application/octet-stream",
                    Metadata = item.Metadata
                });

                if (limit.HasValue && list.Count >= limit.Value) break;
            }
            return list;
        }
        public async Task<IDictionary<string, string>> GetTagsAsync(string fileUrlOrKey, CancellationToken ct = default)
        {
            var blob = await GetBlobClientAsync(fileUrlOrKey, ct);

            try
            {
                var resp = await blob.GetTagsAsync(cancellationToken: ct);
                // Return a case-insensitive copy to make downstream checks friendlier
                return new Dictionary<string, string>(resp.Value.Tags, StringComparer.OrdinalIgnoreCase);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Blob not found → treat as empty tag set
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        public async Task RemoveTagsAsync(string fileUrlOrKey, IEnumerable<string> tagKeys, CancellationToken ct = default)
        {
            var keys = (tagKeys ?? Array.Empty<string>()).ToArray();
            if (keys.Length == 0) return;

            var blob = await GetBlobClientAsync(fileUrlOrKey, ct);

            try
            {
                // Fetch current tags
                var current = new Dictionary<string, string>(
                    (await blob.GetTagsAsync(cancellationToken: ct)).Value.Tags,
                    StringComparer.OrdinalIgnoreCase);

                var changed = false;
                foreach (var k in keys)
                {
                    if (current.Remove(k)) changed = true;
                }

                if (changed)
                {
                    // Overwrite with remaining tags
                    await blob.SetTagsAsync(current, cancellationToken: ct);
                }
                // If nothing changed, no call made
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Blob not found → nothing to remove; no-op
            }
        }

       public async Task<int> DeleteByTagsAsync(
            IReadOnlyDictionary<string, string> tags,
            bool matchAny = false,
            CancellationToken ct = default)
       {
            if (tags is null || tags.Count == 0) return 0;
            string where = BuildWhereClause(_opt.Container, tags, matchAny);

            int deleted = 0;
            await foreach (var item in _svc.FindBlobsByTagsAsync(where, ct))
            {
                ct.ThrowIfCancellationRequested();

                var container = _svc.GetBlobContainerClient(item.BlobContainerName);
                var blob = container.GetBlobClient(item.BlobName);

                try
                {
                    var resp = await blob.DeleteIfExistsAsync(
                        DeleteSnapshotsOption.IncludeSnapshots,
                        conditions: null,
                        cancellationToken: ct);

                    if (resp.Value) deleted++;
                }
                catch (RequestFailedException)
                {
                    // log or ignore as needed
                }
            }
            return deleted;
        }

        private static string BuildWhereClause(
            string containerName,
            IReadOnlyDictionary<string, string> tags,
            bool matchAny)
        {
            // @container = 'my-container' AND (key1 = 'val1' AND/OR key2 = 'val2' ...)
            var sb = new StringBuilder();
            sb.Append($"@container = '{Escape(containerName)}' AND (");

            var sep = matchAny ? " OR " : " AND ";
            bool first = true;
            foreach (var kv in tags)
            {
                if (!first) sb.Append(sep);
                first = false;

                var k = QuoteKey(kv.Key);       // handle special chars in tag keys
                var v = $"'{Escape(kv.Value)}'"; // escape single quotes in values
                sb.Append($"{k} = {v}");
            }
            sb.Append(')');
            return sb.ToString();
        }

        private static string Escape(string s) => s.Replace("'", "''");

        // Tag keys can be bare identifiers if [A-Za-z_][A-Za-z0-9_]*; otherwise quote with "
        private static string QuoteKey(string key)
        {
            return Regex.IsMatch(key, "^[A-Za-z_][A-Za-z0-9_]*$")
                ? key
                : $"\"{key.Replace("\"", "\"\"")}\"";
        }
    }
}
