using Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Application.ProductViewService.Abstractions
{
    public interface IProductViewDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductView> ProductViews { get; set; }
        DbSet<ProductViewCredential> ProductViewCredentials { get; set; }
        DbSet<User> Users { get; set; }
    }
}
