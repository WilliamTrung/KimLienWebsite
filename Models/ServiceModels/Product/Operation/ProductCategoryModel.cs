using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Product.Operation
{
    public class ProductCategoryModel
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
