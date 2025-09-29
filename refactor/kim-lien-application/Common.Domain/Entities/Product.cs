using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User Creator { get; set; } = null!;
        [ForeignKey(nameof(ModifiedBy))]
        public User? Modifier { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public virtual ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();
        public virtual ICollection<ProductFavor> ProductFavors { get; set; } = new List<ProductFavor>();
    }
    public static class ProductExtension
    {
        public static List<Category> Categories(this Product product)
        {
            if (product.ProductCategories is null || !product.ProductCategories.Any()) {
                return new List<Category>();
            }
            else
            {
                return product.ProductCategories.SelectMany(pc => GetCategories(pc.Category)).Distinct().ToList();
            }
        }
        private static List<Category> GetCategories(Category category)
        {
            var categories = new List<Category> { category };
            if (category.Parent != null)
            {
                categories.AddRange(GetCategories(category.Parent));
            }
            return categories;
        }
        public static List<ProductViewCredential> ProductViewCredentials(this Product product) => product.ProductViews?.SelectMany(pc => pc.ProductViewCredentials).ToList() ?? new List<ProductViewCredential>();
    }
}
