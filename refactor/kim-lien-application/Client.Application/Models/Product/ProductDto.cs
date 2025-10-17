using Common.Kernel.Extensions;
using Common.Kernel.Models.Implementations;

namespace Client.Application.Models.Product
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid Id { get; set; }
        public List<ProductCategoryDto> Categories { get; set; } = new List<ProductCategoryDto>();
        public List<AssetDto> Images { get; set; } = new List<AssetDto>();
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public long Slug { get; set; }
        public string SlugRoute => $"{Name.RemoveAccent().ToLower().Replace(" ", "-")}_{Slug}";
    }
}
