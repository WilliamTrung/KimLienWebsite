using Common.Kernel.Extensions;
using Common.Kernel.Models.Implementations;

namespace Client.Application.Models.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public CategoryDto? Parent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public long Slug { get; set; }
        public string SlugUrl => $"{Name.Replace(" ", "-").RemoveAccent().ToLower()}_{Slug}";
        public List<AssetDto> Images { get; set; } = new List<AssetDto>();
    }
}
