using Common.Domain.Entities;
using Common.Infrastructure.DbContext.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Client.Application.Abstractions
{
    public interface IClientDbContext : IDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductView> ProductViews { get; set; }
        DbSet<ProductViewCredential> ProductViewCredentials { get; set; }
        DbSet<ProductFavor> ProductFavors { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
    }
}
