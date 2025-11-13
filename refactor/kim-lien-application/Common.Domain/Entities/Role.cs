using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Common.Domain.Entities
{
    [Table("AspNetRoles")]
    public class Role : IdentityRole<Guid>
    {
    }
}
