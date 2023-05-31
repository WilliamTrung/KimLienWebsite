using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.DTOs
{
    public class ProductView
    {
        public Guid Id { get; set; }    
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IList<string>? Categories { get; set; } = null!;
        public IList<string>? Pictures { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }
    }
}
