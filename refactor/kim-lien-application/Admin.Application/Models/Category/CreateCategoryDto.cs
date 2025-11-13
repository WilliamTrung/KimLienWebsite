using System.ComponentModel.DataAnnotations;
using Common.Kernel.Models.Implementations;

namespace Admin.Application.Models.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
        public List<AssetDto>? Pictures { get; set; }
    }
}
