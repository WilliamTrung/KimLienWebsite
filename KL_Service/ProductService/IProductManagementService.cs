using KL_ManagementFeature;
using KL_Service.StorageService;
using Microsoft.AspNetCore.Http;
using Models.ApiParams.Product;
using Models.ServiceModels.Categories;
using Models.ServiceModels.Product.Operation;
using Models.ServiceModels.Product.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_Service.ProductService
{
    public interface IProductManagementService
    {
        IEnumerable<ProductAdminViewModel> GetProductAdminView();
        ProductAdminViewModel GetProductAdminById(Guid id);
        Task<Guid> AddProduct(ProductAddApiModel model);
        Task AddCategoryToProduct(ProductCategoryAddModel model);
        Task RemoveCategoryFromProduct(ProductCategoryModel model);
        Task ModifyProduct(ProductModifyModel model);
        IEnumerable<CategoryAdminViewModel> GetMainCategories(Guid productId);
        IEnumerable<CategoryAdminViewModel> GetSubCategories(Guid productId, Guid parentId);
        //need funtions to modify product images 
        //add new images (append to last)
        Task AddImage(Guid productId, List<IFormFile> images);
        //adjust image position
        Task AdjustImagesPosition(Guid productId, string[] images);
        Task DeleteImage(Guid productId, string imageUrl);
    }
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductContainer _imageContainer;
        private readonly IProductManagementFeature _productManagementFeature;
        public ProductManagementService(IProductManagementFeature productManagementFeature, IProductContainer imageContainer)
        {
            _productManagementFeature = productManagementFeature;
            _imageContainer = imageContainer;
        }
        public Task AddCategoryToProduct(ProductCategoryAddModel model)
        {
            return _productManagementFeature.AddCategoryToProduct(model);
        }

        public async Task<Guid> AddProduct(ProductAddApiModel model)
        {
            List<string> imageUrls = new List<string>();
            foreach (var image in model.Pictures)
            {
                string url = await _imageContainer.UploadFile(image);
                imageUrls.Add(url);
            }
            ProductAddModel addModel = new ProductAddModel() { 
                Categories = model.Categories,
                Name = model.Name,
                Pictures = imageUrls    
            };
            return await _productManagementFeature.AddProduct(addModel);
        }

       
        public IEnumerable<CategoryAdminViewModel> GetMainCategories(Guid productId)
        {
            return _productManagementFeature.GetMainCategories(productId);
        }

        public ProductAdminViewModel GetProductAdminById(Guid id)
        {
            return _productManagementFeature.GetProductAdminById(id);
        }

        public IEnumerable<ProductAdminViewModel> GetProductAdminView()
        {
            return _productManagementFeature.GetProductAdminView();
        }

        public IEnumerable<CategoryAdminViewModel> GetSubCategories(Guid productId, Guid parentId)
        {
            return _productManagementFeature.GetSubCategories(productId, parentId);
        }

        public Task ModifyProduct(ProductModifyModel model)
        {
            return _productManagementFeature.ModifyProduct(model);
        }

        public Task RemoveCategoryFromProduct(ProductCategoryModel model)
        {
            return _productManagementFeature.RemoveCategoryFromProduct(model);
        }

        public async Task DeleteImage(Guid productId, string imageUrl)
        {
            await _imageContainer.DeleteFile(imageUrl);
            await _productManagementFeature.DeleteImage(productId, imageUrl);            
        }
        public Task AdjustImagesPosition(Guid productId, string[] images)
        {
            return _productManagementFeature.AdjustImagesPosition(productId, images);
        }
        public async Task AddImage(Guid productId, List<IFormFile> images)
        {
            var product = _productManagementFeature.GetProductAdminById(productId);
            if(product != null)
            {
                List<string> imageUrls = new List<string>();
                //execute action
                foreach (var image in images)
                {
                    var url = await _imageContainer.UploadFile(image);
                    imageUrls.Add(url);
                }
                await _productManagementFeature.AddImage(productId, imageUrls);
            }
            
        }
    }
}
