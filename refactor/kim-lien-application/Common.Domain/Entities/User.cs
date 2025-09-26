using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class User : BaseEntity<Guid>, IAuditEntity
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User Creator { get; set; } = null!;
        [ForeignKey(nameof(ModifiedBy))]
        public User? Modifier { get; set; }
        public virtual ICollection<ProductFavor> ProductFavors { get; set; } = new List<ProductFavor>();
        public virtual ICollection<ProductViewCredential> ProductViewCredentials { get; set; } = new List<ProductViewCredential>();
    }
}
