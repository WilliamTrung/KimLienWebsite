using Microsoft.AspNetCore.Identity;

namespace Common.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public string Region { get; set; } = null!;
    }
}
