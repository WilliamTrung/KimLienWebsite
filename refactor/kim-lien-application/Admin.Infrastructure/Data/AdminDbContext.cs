using Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Data
{
    public class AdminDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductView> ProductViews { get; set; }
        public virtual DbSet<ProductViewCredential> ProductViewCredentials { get; set; }
        public virtual DbSet<ProductFavor> ProductFavors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }


        public AdminDbContext(DbContextOptions options) : base(options)
        {
        }

        public AdminDbContext()
        {
        }

    }
}
