using Authen.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Authen.Infrastructure.Data
{
    public class AuthenIdentityDbContext : IdentityDbContext<Common.Domain.Entities.User, Common.Domain.Entities.Role, Guid>
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
        public AuthenIdentityDbContext(DbContextOptions<AuthenIdentityDbContext> options) : base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
