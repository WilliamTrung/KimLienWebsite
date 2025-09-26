using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Entities
{
    public class ProductFavor
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
