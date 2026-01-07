using Common.Kernel.Models.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Common.Infrastructure.Interceptor
{
    /// <summary>
    /// Interceptor for soft delete functionality.
    /// When an entity implementing IDeleteEntity is removed, instead of deleting it,
    /// this interceptor sets IsDeleted = true and changes the state to Modified.
    /// </summary>
    public class SoftDeleteEntityInterceptor : SaveChangesInterceptor
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

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker.Entries<IDeleteEntity>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                // Set IsDeleted to true instead of actually deleting
                entry.Entity.IsDeleted = true;
                
                // Change state from Deleted to Modified so EF Core will update the entity
                entry.State = EntityState.Modified;
            }
        }
    }
}
