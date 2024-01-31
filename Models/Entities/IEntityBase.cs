using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public interface IEntityIdentifier
    {
        Guid Id { get; }
    }
    public interface IAuditEntity
    {
        DateTime CreatedDate { get; set; }
        DateTime LastModifiedDate { get; set; }
    }
    public interface IDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
