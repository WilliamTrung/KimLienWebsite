namespace Common.Infrastructure.Storage.Azure
{
    public sealed class AzureBlobOption
    {
        /// <summary>Azure Storage connection string.</summary>
        public string ConnectionString { get; set; } = default!;

        /// <summary>Default container name (e.g., "products").</summary>
        public string Container { get; set; } = default!;

        /// <summary>Whether the container is configured for public blob access (optional hint).</summary>
        public bool ContainerIsPublic { get; set; } = false;
    }
}
