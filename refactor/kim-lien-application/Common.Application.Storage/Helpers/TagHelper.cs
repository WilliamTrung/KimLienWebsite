using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Storage.Helpers
{
    public static class TagHelper
    {
        public const string StateTag = "state";
        public const string DeleteAtTag = "deleteAt"; // ISO-8601 UTC so string comparison matches time order

        public static (IDictionary<string, string> Tags, IDictionary<string, string> Metadata)
            BuildDeletedTags(DateTimeOffset deleteAtUtc)
        {
            var iso = deleteAtUtc.ToString("o"); // e.g., 2025-10-10T09:30:00.0000000Z

            // Tags are queryable with FindBlobsByTags
            var tags = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [StateTag] = "deleted",
                [DeleteAtTag] = iso
            };

            // (Optional) store same info in metadata (not queryable by tag search, but handy to inspect)
            var meta = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [StateTag] = "deleted",
                [DeleteAtTag] = iso
            };

            return (tags, meta);
        }
    }
}
