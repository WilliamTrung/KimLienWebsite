using Models.ServiceModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Product.View
{
    public class ProductAdminViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public List<string> Pictures { get; set; } = null!;
        public int ViewCount { get; set; }
        public List<CategoryAdminViewModel> Categories { get; set; } = null!;
    }
}
