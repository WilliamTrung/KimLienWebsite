using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace KL_Core
{
    public class KimLienContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        public KimLienContext(DbContextOptions<KimLienContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if(optionsBuilder != null)
            //{
            //    optionsBuilder.UseSqlServer("server=localhost;database=kimliendb_rework;uid=sa;pwd=123;TrustServerCertificate=True");
            //}            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.Property(c => c.Role).HasDefaultValue(Models.Enum.Role.General);
            });
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
                e.HasKey(x => x.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.Property(e => e.IsDeleted).HasDefaultValue(false);
            });
            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Products");
                e.HasKey(x => x.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.Property(e => e.IsDeleted).HasDefaultValue(false);            
            });
            modelBuilder.Entity<ProductCategory>(e =>
            {
                e.ToTable("ProductCategories");
                e.HasKey(x => new { x.ProductId , x.CategoryId });
                e.HasOne(c => c.Product)
                    .WithMany(c => c.ProductCategories)
                    .HasForeignKey(c => c.ProductId);
                e.HasOne(c => c.Category)
                   .WithMany(c => c.ProductCategories)
                   .HasForeignKey(c => c.CategoryId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}