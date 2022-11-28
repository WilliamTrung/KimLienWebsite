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
    }
}
