using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Product.Operation
{
    public class ProductCategoryAddModel
    {
        public Guid ProductId { get; set; }
        public List<Guid> Categories { get; set; } = null!;
    }
}
