using Common.Kernel.TenantProvider.Abstractions;

namespace Common.Infrastructure.Interceptor.TenantQuery
{
    public interface ITenantDbContext
    {
        ITenantProvider TenantProvider { get; }
    }
}
