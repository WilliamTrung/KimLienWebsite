namespace Common.Kernel.TenantProvider.Implementations
{
    public class TenantProvider
    {
        public static TenantProvider Instance { get; set; } = null!;
        private string? _tenantId { get; set; }
        public string TenantId => _tenantId ?? string.Empty;

        public void SetTenantId(string? tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
