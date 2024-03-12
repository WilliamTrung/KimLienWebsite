using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Product.View
{
    public class ProductCustomerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime LastModifiedDate { get; set; }
        public List<string> Pictures { get; set; } = new List<string>();
        public int ViewCount { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}
