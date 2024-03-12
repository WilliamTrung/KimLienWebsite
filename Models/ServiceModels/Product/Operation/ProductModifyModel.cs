using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Product.Operation
{
    public class ProductModifyModel
    {
        public Guid Id { get; }
        public string? Name { get; set; } = null!;
        public bool? IsDeleted { get; set; }
        
    }
}
