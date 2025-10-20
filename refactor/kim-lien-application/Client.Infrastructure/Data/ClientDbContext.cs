using Common.Domain.Entities;
using Common.Infrastructure.Interceptor.TenantQuery.Model;
using Common.Kernel.TenantProvider.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Client.Infrastructure.Data
{
    public class ClientDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });
            modelBuilder.Entity<ITenantEntity>().HasQueryFilter(x => x.TenantId == TenantProvider.Instance.TenantId);
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


        public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
        {
        }
    }
}
