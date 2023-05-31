using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repository
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
    {
        Task DeleteByCategory(Guid categoryId);
    }
}
