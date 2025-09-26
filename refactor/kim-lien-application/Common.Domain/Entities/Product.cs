using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations;

namespace Common.Domain.Entities
{
    public class Product : BaseEntity<Guid>, IAuditEntity, IDeleteEntity
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
        public virtual ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();
    }
    public static class ProductExtension
    {
        public static List<Category> Categories(this Product product) => product.ProductCategories?.Select(pc => pc.Category).ToList() ?? new List<Category>();
        public static List<ProductViewCredential> ProductViewCredentials(this Product product) => product.ProductViews?.SelectMany(pc => pc.ProductViewCredentials).ToList() ?? new List<ProductViewCredential>();
    }
}
