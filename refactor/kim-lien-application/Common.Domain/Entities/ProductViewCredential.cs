using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using Common.Kernel.Parameters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ProductViewCredential : BaseEntity<Guid>, IAuditEntity
    {
        public int ViewCount { get; set; }
        public ProductViewType ViewType { get; set; }
        public Guid ProductViewId { get; set; }
        public Guid? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        [ForeignKey(nameof(ProductViewId))]
        public ProductView ProductView { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
