using Common.Kernel.Models.Implementations;
using LinqKit;
using System.Linq.Expressions;

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
            List<string>? input,
            Func<IQueryable<TEntity>, string, IQueryable<TEntity>> nameFilter)
            where TEntity : BaseSlugEntity<TKey>
            where TKey : IParsable<TKey>
        {
            if (!(input?.Any() ?? false))
                return query;
            var baseQuery = query;
            var combined = baseQuery.Where(_ => false); // empty set

            foreach (var s in input)
            {
                // call your single-input overload
                var qItem = baseQuery.QuerySlug<TEntity, TKey>(s!, nameFilter);
                combined = combined.Union(qItem); // OR semantics, distinct rows
            }
            return combined;
        }
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
            where TEntity : BaseSlugEntity<TKey>
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
        /// <summary>
        /// Build a predicate for slug lookup:
        /// - If input parses to TKey => x => x.Id == key
        /// - Else split "<name>_<number?>"
        ///     - if numeric tail parses to long and numericSelector != null => x => numericSelector(x) == num
        ///     - otherwise => namePredicateFactory(name)  (e.g., ILIKE/Unaccent on Name)
        /// </summary>
        public static Expression<Func<TEntity, bool>> BuildSlugQuery<TEntity, TKey>(
            string? input,
            Func<string, Expression<Func<TEntity, bool>>> namePredicateFactory)
            where TEntity : BaseSlugEntity<TKey>
            where TKey : IParsable<TKey>
        {
            // default: no filter
            if (string.IsNullOrWhiteSpace(input))
                return _ => true;

            var s = input.Trim();

            // 1) Try parse as Id (TKey)
            if (TKey.TryParse(s, provider: null, out var key))
            {
                // x => x.Id.Equals(key)
                return x => x.Id!.Equals(key);
            }

            // 2) Split "<name>_<number?>"
            var parts = s.Split('_', 2, StringSplitOptions.RemoveEmptyEntries);

            // 2a) numeric tail present and we have a selector -> prioritize numeric
            if (parts.Length == 2 && long.TryParse(parts[1].Trim(), out var num))
            {
                //hence TEntity always has a property long Slug
                //Expression here should be x => x.Slug == num
                return SlugEquals<TEntity>(num, nameof(BaseSlugEntity<TKey>.Slug));
            }

            // 2b) otherwise use the name predicate produced by the factory
            var namePart = parts[0].Trim();
            return namePredicateFactory(namePart);
        }
        public static Expression<Func<TEntity, bool>> BuildSlugQuery<TEntity, TKey>(
            List<string>? input,
            Func<string, Expression<Func<TEntity, bool>>> namePredicateFactory)
          where TEntity : BaseSlugEntity<TKey>
          where TKey : IParsable<TKey>
        {
            var init = PredicateBuilder.New<TEntity>(_ => true);
            if (!(input?.Any() ?? false))
                return init;
            foreach (var s in input)
            {
                // call your single-input overload
                var qItem = BuildSlugQuery<TEntity, TKey>(s!, namePredicateFactory);
                init = init.Or(qItem); // OR semantics, distinct rows
            }
            return init;
        }
        private static Expression<Func<TEntity, bool>> SlugEquals<TEntity>(long num, string slugPropertyName)
        {
            var p = Expression.Parameter(typeof(TEntity), "x");

            // x.Slug
            var slug = Expression.PropertyOrField(p, slugPropertyName); // throws if not found

            // constant on the right with the same type as Slug (handles long?)
            Expression right = slug.Type == typeof(long)
                ? Expression.Constant(num, typeof(long))
                : Expression.Convert(Expression.Constant(num, typeof(long)), slug.Type);

            // x.Slug == num
            var body = Expression.Equal(slug, right);

            return Expression.Lambda<Func<TEntity, bool>>(body, p);
        }
    }
}
