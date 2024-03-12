using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Categories
{
    public class CategoryCustomerModel
    {
        public string Name { get; set; } = null!;
        public List<CategoryCustomerModel>? Children { get; set; } = null;
    }
}
