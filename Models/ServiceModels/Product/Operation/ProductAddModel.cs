using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Product.Operation
{
    public class ProductAddModel
    {
        public string Name { get; set; } = null!;
        public List<string> Pictures { get; set; } = null!;
        public List<Guid> Categories { get; set; } = null!;
    }
}
