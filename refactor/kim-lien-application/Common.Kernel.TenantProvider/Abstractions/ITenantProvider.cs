namespace Common.Kernel.TenantProvider.Abstractions
{
    public interface ITenantProvider
    {
        string TenantId { get; }
        void SetTenantId(string? tenantId);
    }
}
