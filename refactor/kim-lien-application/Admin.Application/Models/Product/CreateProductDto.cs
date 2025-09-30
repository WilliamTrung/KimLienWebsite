using System.ComponentModel.DataAnnotations;

namespace Admin.Application.Models.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Guid> CategoryIds { get; set; } = null!; // List of associated category IDs
        public string PictureContainer { get; set; } // Comma-separated list of picture URLs

    }
}
