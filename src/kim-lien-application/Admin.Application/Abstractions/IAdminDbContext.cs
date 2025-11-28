using Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Application.Abstractions
{
    public interface IAdminDbContext
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
