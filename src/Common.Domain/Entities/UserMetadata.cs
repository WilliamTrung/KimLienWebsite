using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class UserMetadata
    {
        [Key]
        public Guid UserId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastActiveAt { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
