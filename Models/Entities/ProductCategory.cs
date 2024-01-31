using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class ProductCategory
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
