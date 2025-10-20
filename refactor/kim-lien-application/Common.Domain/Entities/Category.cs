using Common.Extension;
using Common.Infrastructure.Interceptor.TenantQuery.Model;
using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Common.Domain.Entities
{
    public class Category : BaseEntity<Guid>, IAuditEntity, IDeleteEntity, IQueryEntity, ITenantEntity
    {
        [Required]
        public string Name { get; set; } = null!;
        [AllowNull]
        public Guid? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Category? Parent { get; set; }
        public virtual IList<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User Creator { get; set; } = null!;
        [ForeignKey(nameof(ModifiedBy))]
        public User? Modifier { get; set; }
        public bool IsDeleted { get; set; }
        public string BareName { get; set; } = null!;
        public string TenantId { get; set; } = null!;
        public JsonDocument? Pictures { get; set; }
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
        public JsonDocument? Payload { get; set; }
    }
    public static class CategoryExtension
    {
        public static readonly string QueryChildren =
            "$@WITH tree AS (SELECT * FROM [Categories] WHERE [Id] = {parentId} UNION ALL SELECT c.* FROM [Categories] c JOIN tree t ON c.[ParentId] = t.[Id]) SELECT * FROM tree WHERE [Id] <> {parentId};";
        public static readonly string QueryParents = 
            "$@WITH tree AS (SELECT * FROM [Categories] WHERE [Id] = {childId} UNION ALL SELECT c.* FROM [Categories] c JOIN tree t ON c.[Id] = t.[ParentId]) SELECT * FROM tree WHERE [Id] <> {childId};";
        public static List<Product> Products(this Category category) => category.ProductCategories?.Select(pc => pc.Product).ToList() ?? new List<Product>();
        
    }
}
