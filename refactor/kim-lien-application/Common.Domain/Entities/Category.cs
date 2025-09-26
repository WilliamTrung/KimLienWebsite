using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Common.Domain.Entities
{
    public class Category : BaseEntity<Guid>
    {
        [Required]
        public string Name { get; set; } = null!;
        [AllowNull]
        public Guid? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Category? Parent { get; set; }
        public virtual IList<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
    public static class CategoryExtension
    {
        public static List<Product> Products(this Category category) => category.ProductCategories?.Select(pc => pc.Product).ToList() ?? new List<Product>();
    }
}
