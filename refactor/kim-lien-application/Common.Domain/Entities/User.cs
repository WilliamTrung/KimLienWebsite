using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace Common.Domain.Entities
{
    [Table("AspNetUsers")]
    public class User : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public string Region { get; set; } = null!;
        public JsonDocument? Asset { get; set; }
    }
}
