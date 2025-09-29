using Common.Kernel.Constants;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Infrastructure.Pagination
{
    public static class PaginationExtension
    {
        /// <summary>
        /// Return queryable, pageIndex, pageSize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static (IQueryable<T> Query, int PageIndex, int PageSize) ToPaginationQuery<T>(this IQueryable<T> queryable, int pageIndex = Constant.MinPageIndexValue, int pageSize = Constant.MinPageSizeValue)
        {
            pageIndex = pageIndex - 1;
            pageIndex = Math.Max(pageIndex, Constant.MinPageIndexValue);
            pageSize = Math.Clamp(pageSize, Constant.MinPageSizeValue, Constant.MaxPageSizeValue);

            return (queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize), 
                pageIndex, 
                pageSize);
        }

        /// <summary>
        /// Sắp xếp lại data trước khi pagination, có hỗ trợ nested property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sortField"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, List<string> sortFields, bool ascending)
        {
            if (sortFields == null || !sortFields.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var currentQuery = query;

            bool isFirst = true;

            foreach (var field in sortFields)
            {
                Expression propertyAccess = parameter;
                Type currentType = typeof(T);

                foreach (var prop in field.Split('.'))
                {
                    var property = currentType.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property == null)
                        throw new ArgumentException($"Property '{prop}' not found on type '{currentType.Name}'");

                    propertyAccess = Expression.Property(propertyAccess, property);
                    currentType = property.PropertyType;

                    // Handle collection with FirstOrDefault
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(currentType) && currentType != typeof(string))
                    {
                        var elementType = currentType.GetInterfaces()
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                            ?.GetGenericArguments()[0] ?? typeof(object);

                        propertyAccess = Expression.Call(
                            typeof(Enumerable),
                            "FirstOrDefault",
                            new[] { elementType },
                            propertyAccess
                        );

                        currentType = elementType;
                    }
                }

                if (propertyAccess.Type == typeof(string))
                {
                    propertyAccess = Expression.Call(propertyAccess, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
                }
                var lambda = Expression.Lambda(propertyAccess, parameter);

                var methodName = isFirst
                    ? ascending ? "OrderBy" : "OrderByDescending"
                    : ascending ? "ThenBy" : "ThenByDescending";

                var resultExp = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new Type[] { typeof(T), currentType },
                    currentQuery.Expression,
                    Expression.Quote(lambda)
                );

                currentQuery = currentQuery.Provider.CreateQuery<T>(resultExp);
                isFirst = false;
            }

            return currentQuery;
        }
        public static IQueryable<T> ApplySorting<T, TKey>(
              this IQueryable<T> query,
              Expression<Func<T, TKey>> keySelector,
              bool ascending)
        {
            var sortFields = ExtractSortFields(keySelector.Body);
            return query.ApplySorting(sortFields, ascending); // your existing overload
        }

        /// <summary>
        /// Sắp xếp lại data trước khi pagination, có hỗ trợ nested property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sortField"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortField, bool ascending)
        {
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "x");
            Expression propertyAccess = parameter;

            foreach (var prop in sortField.Split('.'))
            {
                var property = entityType.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    return query;

                var isEnumerable = typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType)
                                   && property.PropertyType != typeof(string);

                propertyAccess = Expression.Property(propertyAccess, property);
                entityType = property.PropertyType;

                if (isEnumerable)
                {
                    // Use FirstOrDefault for collection access
                    var elementType = property.PropertyType.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        ?.GetGenericArguments()[0] ?? typeof(object);

                    propertyAccess = Expression.Call(
                        typeof(Enumerable),
                        "FirstOrDefault",
                        new Type[] { elementType },
                        propertyAccess
                    );

                    entityType = elementType;
                }
            }

            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var orderByCall = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), entityType },
                query.Expression,
                Expression.Quote(orderByExp)
            );

            return query.Provider.CreateQuery<T>(orderByCall);
        }
        private static List<string> ExtractSortFields(Expression expr)
        {
            var fields = new List<string>();
            switch (expr)
            {
                case MemberExpression m:
                    fields.Add(GetMemberPath(m));
                    break;

                case UnaryExpression u when u.NodeType is ExpressionType.Convert
                                             or ExpressionType.ConvertChecked:
                    fields.AddRange(ExtractSortFields(u.Operand));
                    break;

                case NewExpression nx when nx.Members is not null:
                    // new { x.A, x.B, x.User.Name }
                    foreach (var arg in nx.Arguments)
                        fields.AddRange(ExtractSortFields(arg));
                    break;

                default:
                    throw new NotSupportedException($"Unsupported sort key: {expr.NodeType}");
            }
            return fields;
        }
        private static string GetMemberPath(MemberExpression m)
        {
            var stack = new Stack<string>();
            Expression cur = m;
            while (cur is MemberExpression mm)
            {
                stack.Push(mm.Member.Name);
                cur = mm.Expression;
            }
            return string.Join(".", stack); // e.g., "CreatedAt" or "User.Name"
        }
    }
}
