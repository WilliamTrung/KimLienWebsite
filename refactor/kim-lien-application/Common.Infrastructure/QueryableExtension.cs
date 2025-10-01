using Common.Kernel.Models.Implementations;

namespace Common.Infrastructure
{
    public static class QueryableExtension
    {
        /// <summary>
        /// Slug lookup:
        /// - If input parses to TKey => filter by Id.
        /// - Else split on first '_' :
        ///     - if second half parses to long and numericFilter provided => use numeric filter
        ///     - otherwise => use name filter
        /// </summary>
        public static IQueryable<TEntity> QuerySlug<TEntity, TKey>(
            this IQueryable<TEntity> query,
            string? input,
            Func<IQueryable<TEntity>, string, IQueryable<TEntity>> nameFilter)
            where TEntity : BaseEntity<TKey>
            where TKey : IParsable<TKey>
        {
            if (string.IsNullOrWhiteSpace(input))
                return query;

            var s = input.Trim().ToLower();

            // 1) Try parse as Id (TKey)
            if (TKey.TryParse(s, provider: null, out var key))
            {
                // Comparing a captured TKey is translatable by EF
                return query.Where(x => x.Id!.Equals(key));
            }

            // 2) Split "<name>_<number?>"
            var parts = s.Split('_', 2, StringSplitOptions.RemoveEmptyEntries);

            // 2a) If we have a numeric tail and a numeric filter was provided → prioritize numeric
            if (parts.Length == 2 && long.TryParse(parts[1].Trim(), out var num))
            {
                return query.Where(x => x.Slug == num);
            }

            // 2b) Otherwise use the name filter on the first part
            var namePart = parts[0].Trim();
            query = nameFilter.Invoke(query, namePart);
            return query;
        }
    }
}
