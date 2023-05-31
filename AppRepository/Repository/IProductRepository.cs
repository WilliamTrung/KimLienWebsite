using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> GetByProductCategory(List<string> categories, Expression<Func<Product, bool>>? filter = null, Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null, string? includeProperties = null);
    }
}
