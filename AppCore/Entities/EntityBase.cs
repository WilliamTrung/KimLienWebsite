using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Entities
{
    public class EntityBase
    {
        public Guid Id { set; get; }
    }

    public interface IAuditEntity
    {
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
    }

    public interface IDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
