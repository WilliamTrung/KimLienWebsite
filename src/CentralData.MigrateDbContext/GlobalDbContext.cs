using Authen.Domain.Entities;
using Common.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CentralData.MigrateDbContext
{
    public class GlobalDbContext : IdentityDbContext<User, Role, Guid>
    {
        public GlobalDbContext(DbContextOptions<GlobalDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.HasIndex(x => x.TokenHash).IsUnique();
                e.HasIndex(x => new { x.UserId, x.FamilyId });
                e.Property(x => x.TokenHash).HasMaxLength(256);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            });
            modelBuilder.Entity<ProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });
            modelBuilder.HasPostgresExtension("unaccent"); // Npgsql
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductView> ProductViews { get; set; }
        public virtual DbSet<ProductViewCredential> ProductViewCredentials { get; set; }
        public virtual DbSet<ProductFavor> ProductFavors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
