using Common.Kernel.Models.Implementations;

namespace Admin.Application.Models.Product
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid Id { get; set; }
        public List<ProductCategoryDto> Categories { get; set; } = new List<ProductCategoryDto>();
        public List<AssetDto> Images { get; set; } = new List<AssetDto>();

    }
}
