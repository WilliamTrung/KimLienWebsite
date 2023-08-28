using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.DTOs
{
    public class CategoryView
    {
        public string Name { get; set; } = null!;
        public List<CategoryView> Children { get; set; } = new List<CategoryView>();

    }
}
