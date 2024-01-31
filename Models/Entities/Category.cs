using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Category : IEntityIdentifier, IDeleteEntity
    {
        public Guid Id { get; }
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = null!;
    }
}
