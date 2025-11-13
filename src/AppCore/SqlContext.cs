using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    public class SqlContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;

        public SqlContext()
        {
        }

        public SqlContext(DbContextOptions<SqlContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductCategory>().HasKey(p => new { p.ProductId, p.CategoryId });
            modelBuilder.Entity<ProductCategory>().HasOne(pc => pc.Product).WithMany(p => p.ProductCategories).HasForeignKey(pc => new {pc.ProductId});
            modelBuilder.Entity<ProductCategory>().HasOne(pc => pc.Category).WithMany(p => p.ProductCategories).HasForeignKey(pc => new { pc.CategoryId });

        }
    }
}
