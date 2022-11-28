using AppCore;
using AppCore.Entities;
using AppService.Extension;
using AppService.IService;
using AppService.Models;
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
    public class ProductService : BaseService<Product, DTOs.Product>, IProductService
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductService(SqlContext context, IMapper mapper) : base(context, mapper)
        {
            _productCategoryService = new ProductCategoryService(context, mapper);  
        }

        public IEnumerable<ProductModel> CheckCategories(IEnumerable<ProductModel> productModels, List<string> categories)
        {
            var result = new List<ProductModel>();
            foreach(var product in productModels)
            {
                if(CheckCategories(product, categories))
                {
                    result.Add(product);
                }
            }
            return result;
        }
        private bool CheckCategories(ProductModel productModel, List<string> categories)
        {
            foreach(var productcategory in productModel.ProductCategories)
            {
                foreach(string category in categories)
                {
                    if(Helper.MinimalCompareString(productcategory.Category.Name, category))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override void DisableSelfReference(ref Product entity)
        {
            //IDK what to do here yet
        }
        public override Task<IEnumerable<DTOs.Product>> GetDTOs(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, PagingRequest? paging = null)
        {
            var result = base.GetDTOs(filter, includeProperties, paging);
            foreach(var item in result.Result)
            {
                if(item.Pictures != null)
                    item.DeserializedPictures = item.Pictures.Split(",");

            }
            return result;
        }

        public async Task<IEnumerable<ProductModel>> GetProductModels(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, PagingRequest? paging = null)
        {
            var products = await GetDTOs(filter, includeProperties, paging);
            var result = new List<ProductModel>();
            foreach(var product in products)
            {
                var productModel = new ProductModel()
                {
                    Product = product,
                    ProductCategories = (await _productCategoryService.GetDTOs(includeProperties: "Product,Category")).ToList()
                };
                result.Add(productModel);
            }
            return result;
        }
    }
}
