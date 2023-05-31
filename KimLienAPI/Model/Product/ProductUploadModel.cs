using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KimLienAPI.Model.Product
{
    public class ProductUploadModel
    {
        [Required]
        [FromRoute]
        public Guid Id { get; set; }
        
        [Validation.AllowedExtensions("Invalid extension",".jpg",".png",",jpeg")]
        [FromBody]
        public List<IFormFile> Files { get; set; } = null!;
    }
}
