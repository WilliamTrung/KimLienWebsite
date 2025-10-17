using Common.Kernel.Extensions;

namespace Admin.Application.Models.Category
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
    }
}
