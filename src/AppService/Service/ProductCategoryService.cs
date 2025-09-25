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
        /// <summary>
        /// Get all Categories in DB -> [CategoriesDB]
        /// For each in [CategoriesDB] -> [item]
        ///     if [item] exists in [categoryId]
        ///         if [item].IsDeleted == true
        ///             [item].IsDeleted = false
        ///             update [item] in DB
        ///         [categoryId] remove [item]
        /// For each in [categoryId] -> [item]
        ///     add new [Category] to DB
        ///     
        /// </summary>
        /// <param name="product"></param>
        /// <param name="categoryId"></param>
        /// <returns>List Categories in DB</returns>
        public async Task<IEnumerable<DTOs.ProductCategory>> Implement(DTOs.Product product, List<Guid> categoryId)
        {
            //get all
            var categories_db = await GetDTOs(filter: p => p.ProductId == product.Id);
            foreach(var item in categories_db)
            {
                if (categoryId.Contains(item.CategoryId))
                {
                    if(item.IsDeleted == true)
                    {
                        item.IsDeleted= false;
                        var check = await Update(p => p.CategoryId == item.CategoryId && p.ProductId == item.ProductId, item);

                    }
                    categoryId.Remove(item.CategoryId);
                } else
                {
                    //set status to be inactive
                    if(item.IsDeleted == false)
                    {
                        item.IsDeleted= true;
                        var check = await Update(p => p.CategoryId == item.CategoryId && p.ProductId == item.ProductId, item);
                    }
                }
            }
            foreach(var item in categoryId)
            {
                var category = new DTOs.ProductCategory()
                {
                    ProductId = product.Id,
                    CategoryId = item,
                    IsDeleted = false
                };
                var check = await Create(category);
            }
            var result = await GetDTOs(filter: p => p.ProductId == product.Id);
            return result;
            
        }
    }
}
