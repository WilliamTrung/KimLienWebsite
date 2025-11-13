using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.DTOs
{
    public class ProductCategory
    {
        [Required]
        public Guid ProductId;
        [Required]
        public Guid CategoryId;
        [Required]
        public bool IsDeleted { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
