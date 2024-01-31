using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Product : IEntityIdentifier, IDeleteEntity, IAuditEntity
    {
        public Guid Id { get; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get;set; }
        public DateTime LastModifiedDate { get; set; }
        public string Pictures { get; set; } = null!;
        public int ViewCount { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = null!;
    }
}
