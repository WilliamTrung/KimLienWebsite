using Authen.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Role = Common.Domain.Entities.Role;
using User = Common.Domain.Entities.User;

namespace Authen.Infrastructure.Data
{
    public class AuthenIdentityDbContext : IdentityDbContext<User, Role, Guid>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RefreshToken>(e =>
            {
                e.HasIndex(x => x.TokenHash).IsUnique();
                e.HasIndex(x => new { x.UserId, x.FamilyId });
                e.Property(x => x.TokenHash).HasMaxLength(256);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            });
            base.OnModelCreating(builder);
        }
        public AuthenIdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AuthenIdentityDbContext()
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
