using AppCore.Entities;
using AppService.Models;
using AppService.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppService.IService
{
    public interface IProductService : IBaseService<Product, DTOs.Product>
    {
        public Task<IEnumerable<ProductModel>> GetProductModels(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, PagingRequest? paging = null);
        public IEnumerable<ProductModel> CheckCategories(IEnumerable<ProductModel> productModels, List<string> categories);        
    }
}
