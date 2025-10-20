using Common.Kernel.TenantProvider.Abstractions;

namespace Common.Kernel.TenantProvider.Implementations
{
    public class TenantProvider : ITenantProvider
    {
        private string? _tenantId { get; set; }
        public string TenantId => _tenantId ?? string.Empty;

        public void SetTenantId(string? tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
