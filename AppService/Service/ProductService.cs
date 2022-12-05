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
        public IList<string> GetDeserializedPictures(DTOs.Product product)
        {
            IList<string> result = new List<string>();
            if (product.Pictures != null)
                result = product.Pictures.Split(",");
            return result;
        }
        public override Task<IEnumerable<DTOs.Product>> GetDTOs(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, PagingRequest? paging = null)
        {
            var result = base.GetDTOs(filter, includeProperties, paging);
            foreach(var item in result.Result)
            {
                if (item.Pictures != null)
                    item.DeserializedPictures = GetDeserializedPictures(item);
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
        public override async Task<DTOs.Product> Update(Expression<Func<Product, bool>> filter, DTOs.Product dto)
        {
            var find = await GetDTOs(filter: filter);
            var found = find.FirstOrDefault();
            if(found != null)
            {
                //adjust pictures
                if(dto.Pictures != null && dto.Pictures != found.Pictures)
                {
                    var preupdate = found.Pictures;
                    if (preupdate != null)
                    {
                        var primary_pic = preupdate.Split(',')[0];
                        IList<string> deserialized = new List<string>();
                        bool isExist = false;
                        foreach(var pic in dto.Pictures.Split(","))
                        {
                            if(pic != primary_pic)
                            {
                                deserialized.Add(pic);
                            } else
                            {
                                isExist = true;
                            }
                        }
                        if(isExist)
                        {
                            deserialized.Insert(0, primary_pic);
                        }
                        dto.Pictures = Extension.Helper.MergeListString(deserialized);
                    }
                }
                
            }
            return await base.Update(filter, dto);
        }

        public async Task<DTOs.Product?> SetPrimaryPicture(Guid productId, string picture)
        {
            //throw new NotImplementedException();
            var find = await GetDTOs(filter: p => p.Id == productId);
            var found = find.FirstOrDefault();
            if(found != null)
            { 
                var deserialized = found.DeserializedPictures;
                if (deserialized != null)
                {
                    deserialized = deserialized.ToList();
                    deserialized.Remove(picture);
                    deserialized.Insert(0, picture);
                    found.Pictures = Extension.Helper.MergeListString(deserialized);
                }
                found = await base.Update(p => p.Id == productId, found);
            }
            return found;
        }
    }
}
