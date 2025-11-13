using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations;

namespace Common.Domain.Authen.Entities
{
    public class Role : BaseEntity<int>
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
