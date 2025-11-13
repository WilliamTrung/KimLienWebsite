using Common.Infrastructure.Interceptor.TenantQuery.Model;
using Common.Kernel.TenantProvider.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Common.Infrastructure.Interceptor.TenantQuery
{
    public class TenantSaveChangeInterceptor : SaveChangesInterceptor
    {
        private readonly TenantProvider _tenant;

        public TenantSaveChangeInterceptor(TenantProvider tenant)
            => _tenant = tenant;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            EnforceTenant(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            EnforceTenant(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void EnforceTenant(DbContext? ctx)
        {
            if (ctx is null) return;
            var tid = _tenant.TenantId;

            foreach (var entry in ctx.ChangeTracker.Entries<ITenantEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Stamp on insert
                        entry.Property(e => e.TenantId).CurrentValue = tid;
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        // Guard: prevent cross-tenant writes
                        var original = entry.Property(e => e.TenantId).OriginalValue;
                        if (original != tid)
                            throw new InvalidOperationException("Cross-tenant modification detected.");
                        break;
                }
            }
        }
    }
}
