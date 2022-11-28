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

        public SqlContext()
        {
        }

        public SqlContext(DbContextOptions<SqlContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=WILLIAMTRUNG\\MYSQL;database=kimliendb;uid=sa;pwd=123;trusted_connection=true");
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property("Id").ValueGeneratedOnAdd();
        }
    }
}
