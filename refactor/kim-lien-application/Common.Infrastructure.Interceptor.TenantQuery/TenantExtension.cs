using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Common.Kernel.TenantProvider.Implementations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Common.Infrastructure.Interceptor.TenantQuery.Model;

namespace Common.Infrastructure.Interceptor.TenantQuery
{
    public static class TenantExtension
    {
        public static void ApplyTenantQueryFilter(this ModelBuilder modelBuilder)
        {
            var tenantId = TenantProvider.Instance.TenantId; // your global context
            var tenantEntityType = typeof(Model.ITenantEntity);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(t => tenantEntityType.IsAssignableFrom(t.ClrType)))
            {
                var clrType = entityType.ClrType;

                // Create expression: (e) => e.TenantId == TenantProvider.Instance.TenantId
                var parameter = Expression.Parameter(clrType, "e");
                var tenantIdProperty = Expression.PropertyOrField(parameter, nameof(ITenantEntity.TenantId));
                var tenantIdConstant = Expression.Constant(tenantId);
                var body = Expression.Equal(tenantIdProperty, tenantIdConstant);
                var lambda = Expression.Lambda(body, parameter);

                // modelBuilder.Entity<T>().HasQueryFilter(lambda)
                var method = typeof(ModelBuilder)
                    .GetMethods()
                    .First(m => m.Name == nameof(ModelBuilder.Entity) && m.IsGenericMethod);
                var generic = method.MakeGenericMethod(clrType);
                var entityBuilder = generic.Invoke(modelBuilder, null)!;

                var hasQueryFilterMethod = entityBuilder.GetType()
                    .GetMethods()
                    .First(m => m.Name == nameof(EntityTypeBuilder.HasQueryFilter));

                hasQueryFilterMethod.Invoke(entityBuilder, new object[] { lambda });
            }
        }
    }
}
