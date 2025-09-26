using Common.Domain.Entities;
using Common.Kernel.Models.Implementations;

namespace Authen.Domain.Entities
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public Guid FamilyId { get; set; }          // same device/session
        public Guid UserId { get; set; }
        public string TokenHash { get; set; } = default!; // store HASH, never plain
        public DateTime ExpiresUtc { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ConsumedUtc { get; set; }  // set on rotation
        public DateTime? RevokedUtc { get; set; }   // manual/global logout
        public string? CreatedByIp { get; set; }
        public string? UserAgent { get; set; }

        public User User { get; set; } = default!;
    }
}
