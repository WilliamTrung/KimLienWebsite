using Microsoft.AspNetCore.Http;
using Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ApiParams.Product
{
    public class ProductAddApiModel
    {
        public string Name { get; set; } = null!;
        [KL_Validation.AllowedExtensions(extensions: new string[] { FileExtension.Image.JPEG, FileExtension.Image.JPG, FileExtension.Image.GIF, FileExtension.Image.SVG, FileExtension.Image.WEPP })]
        public List<IFormFile> Pictures { get; set; } = null!;
        public List<Guid> Categories { get; set; } = null!;
    }
}
