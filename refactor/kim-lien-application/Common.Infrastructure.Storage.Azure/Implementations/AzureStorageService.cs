using System.Net;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Common.Application.Storage.Abstraction;
using Common.Application.Storage.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

                var resp = await blob.UploadAsync(stream, new BlobUploadOptions
                {
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

            var resp = await blob.UploadAsync(content, new BlobUploadOptions
            {
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

            var op = await dest.StartCopyFromUriAsync(sourceUri, cancellationToken: ct);
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
    }
}
