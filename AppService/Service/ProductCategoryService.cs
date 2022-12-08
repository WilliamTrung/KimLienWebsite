using AppCore;
using AppCore.Entities;
using AppService.IService;
using AppService.Paging;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Service
{
    public class ProductCategoryService : BaseService<ProductCategory, DTOs.ProductCategory>, IProductCategoryService
    {
        public ProductCategoryService(SqlContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public override Task<IEnumerable<DTOs.ProductCategory>> GetDTOs(Expression<Func<ProductCategory, bool>>? filter = null, string? includeProperties = null, PagingRequest? paging = null)
        {
            return base.GetDTOs(filter, includeProperties, paging);
        }

        public async Task<IEnumerable<DTOs.ProductCategory>> Implement(DTOs.Product product, List<Guid> categoryId)
        {
            var result = new List<DTOs.ProductCategory>();
            foreach(var id in categoryId)
            {
                var find = await GetDTOs(filter: p => p.ProductId == product.Id && p.CategoryId == id);
                var check = find.FirstOrDefault();
                if (check == null)
                {
                    //create
                    var productCategory = new DTOs.ProductCategory()
                    {
                        ProductId = product.Id,
                        CategoryId = id
                    };
                    var created = await Create(productCategory);
                    result.Add(created);
                }
                else
                {
                    //update
                    check.IsDeleted = true;
                    var updated = await Update(filter: p => p.ProductId == check.ProductId && p.CategoryId == id, check);
                    result.Add(updated);
                }
            }
            return result;
            
        }
    }
}
