using Common.Domain.Entities;
using Common.Infrastructure.Interceptor.TenantQuery.Model;
using Common.Kernel.TenantProvider.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Data
{
    public class AdminDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });
            modelBuilder.Entity<ITenantEntity>().HasQueryFilter(e => e.TenantId == TenantProvider.Instance.TenantId);
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductView> ProductViews { get; set; }
        public virtual DbSet<ProductViewCredential> ProductViewCredentials { get; set; }
        public virtual DbSet<ProductFavor> ProductFavors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }


        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {
        }
    }
}
