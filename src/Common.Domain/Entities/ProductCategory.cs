using Common.Kernel.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Common.Domain.Entities
{
    public class ProductCategory : IDeleteEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
