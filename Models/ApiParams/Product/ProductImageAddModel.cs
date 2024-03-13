using KL_Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ApiParams.Product
{
    public class ProductImageAddModel
    {
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Please select image(s).")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensionsAttribute(".jpg", ".png")]
        public List<IFormFile> Images { get; set; } = null!;
    }
}
