using AppCore;
using AppCore.Entities;
using Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repository.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(SqlContext context) : base(context)
        {
        }
        public override IEnumerable<Product> Get(Expression<Func<Product, bool>>? filter = null, Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null, string? includeProperties = null)
        {
            var query = _dbSet.AsQueryable();
            if (query.FirstOrDefault() != null && query.FirstOrDefault() is Product)
            {
                query = query.OrderByDescending(p => (p as Product).ModifiedDate);
            }

            query = query.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category);

            if (includeProperties != null)
            {
                foreach (string property in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.AsEnumerable();
        }

        public IEnumerable<Product> GetByProductCategory(List<string> categories, Expression<Func<Product, bool>>? filter = null, Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null, string? includeProperties = null)
        {
            var query = _dbSet.AsQueryable();
            if (query.FirstOrDefault() != null)
            {
                query = query.OrderByDescending(p => (p as Product).ModifiedDate);
            }

            query = query.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Where(p => p.ProductCategories.Any(pc => categories.Any(c => StringExtension.MinimalCompareString(c, pc.Category.Name))));

            if (includeProperties != null)
            {
                foreach (string property in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.AsEnumerable();
        }
    }
}
