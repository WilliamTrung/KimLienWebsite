using Common.Extension;
using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Common.Domain.Entities
{
    public class Product : BaseEntity<Guid>, IAuditEntity, IDeleteEntity, IQueryEntity
    {
        public string BareName { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;

        public JsonDocument? Pictures { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User Creator { get; set; } = null!;
        [ForeignKey(nameof(ModifiedBy))]
        public User? Modifier { get; set; }
        public string Status { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public virtual ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();
        public virtual ICollection<ProductFavor> ProductFavors { get; set; } = new List<ProductFavor>();
        [NotMapped]
        private List<AssetDto>? _assets;
        [NotMapped]
        public List<AssetDto> PictureAssets
        {
            get
            {
                if (_assets is null)
                {
                    if (Pictures is null)
                    {
                        _assets = new List<AssetDto>();
                    }
                    else
                    {
                        _assets = JsonConvert.DeserializeObject<List<AssetDto>>(Pictures.RootElement.GetRawText()) ?? new List<AssetDto>();
                    }
                }
                return _assets;
            }
            set
            {
                Pictures = value.ToDocument();
                _assets = value;
            }
        }
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
