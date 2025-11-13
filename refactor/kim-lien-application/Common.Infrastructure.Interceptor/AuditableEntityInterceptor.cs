using Common.Kernel.Models.Abstractions;
using Common.RequestContext.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Common.Infrastructure.Interceptor
{
    public class AuditableEntityInterceptor(TimeProvider dateTime, IRequestContext requestContext) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;
            var utcNow = dateTime.GetUtcNow();
            foreach (var entry in context.ChangeTracker.Entries<IAuditEntity>())
            {
                if (entry.State is EntityState.Added)
                {
                    if (Guid.TryParse(requestContext.Data.UserId, out var userId))
                    {
                        entry.Entity.CreatedBy = userId;
                    }
                    entry.Entity.CreatedDate = utcNow.UtcDateTime;
                }
                else if (entry.State is EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    if (Guid.TryParse(requestContext.Data.UserId, out var userId))
                    {
                        entry.Entity.ModifiedBy = userId;
                    }
                    entry.Entity.ModifiedDate = utcNow.UtcDateTime;
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
