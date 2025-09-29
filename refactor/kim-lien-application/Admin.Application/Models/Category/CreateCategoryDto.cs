using System.ComponentModel.DataAnnotations;

namespace Admin.Application.Models.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
    }
}
