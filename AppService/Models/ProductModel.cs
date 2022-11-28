using AppService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Models
{
    public class ProductModel
    {
        public Product Product { get; set; } = null!;
        public List<ProductCategory> ProductCategories { get; set; } = null!;
    }
}
