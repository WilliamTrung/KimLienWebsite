using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Role = Common.Domain.Entities.Role;
using User = Common.Domain.Entities.User;

namespace Authen.Infrastructure.Data
{
    public class AuthenIdentityDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AuthenIdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AuthenIdentityDbContext()
        {
        }
    }
}
