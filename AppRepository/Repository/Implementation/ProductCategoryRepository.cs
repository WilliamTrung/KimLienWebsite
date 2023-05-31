using AppCore;
using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repository.Implementation
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(SqlContext context) : base(context)
        {
        }

        public Task DeleteByCategory(Guid categoryId)
        {
            var categories = Get(filter: pc => pc.CategoryId== categoryId);
            _dbSet.RemoveRange(categories);
            return Task.CompletedTask;
        }
    }
}
