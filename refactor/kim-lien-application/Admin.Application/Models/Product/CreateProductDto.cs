using Common.Kernel.Models.Implementations;

namespace Admin.Application.Models.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Guid> CategoryIds { get; set; } = null!; // List of associated category IDs
        public List<AssetDto> Images { get; set; } = null!;

    }
}
