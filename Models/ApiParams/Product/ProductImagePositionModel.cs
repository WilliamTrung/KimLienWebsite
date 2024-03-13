using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ApiParams.Product
{
    public class ProductImagePositionModel
    {
        public Guid ProductId { get; set; }
        public string[] Images { get; set; } = null!;
    }
}
