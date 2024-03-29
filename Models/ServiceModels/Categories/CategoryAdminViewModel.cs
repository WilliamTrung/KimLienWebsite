using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Categories
{
    public class CategoryAdminViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public ICollection<CategoryAdminViewModel>? Children { get; set; }

    }
}
