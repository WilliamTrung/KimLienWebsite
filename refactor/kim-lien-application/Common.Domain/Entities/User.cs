using Microsoft.AspNetCore.Identity;

namespace Common.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
    }
}
