using Microsoft.AspNetCore.Http;
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
        public List<IFormFile> Pictures { get; set; } = null!;
        public List<Guid> Categories { get; set; } = null!;
    }
}
