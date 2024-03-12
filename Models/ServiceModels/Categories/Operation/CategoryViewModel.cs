using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Categories.Operation
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public ICollection<CategoryViewModel>? Children { get; set; }
    }
}
