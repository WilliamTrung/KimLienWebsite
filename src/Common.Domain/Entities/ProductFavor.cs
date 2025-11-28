using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ProductFavor : BaseEntity<Guid>, IAuditEntity, IDeleteEntity
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
