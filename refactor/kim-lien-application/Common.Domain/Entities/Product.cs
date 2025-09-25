using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class Product : BaseEntity, IAuditEntity, IDeleteEntity
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Pictures { get; set; } = null!; // Comma-separated list of picture URLs
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        [NotMapped]
        public List<Category> Categories => ProductCategories?.Select(pc => pc.Category).ToList() ?? new List<Category>();
    }
}
