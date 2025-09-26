using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations;

namespace Common.Domain.Entities
{
    public class Role : BaseEntity<int>, IAuditEntity
    {
        [Required]
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
