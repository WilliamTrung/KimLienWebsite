using KL_ProductCustomerFeature;
using KL_Repository.UnitOfWork;
using Models.ServiceModels.Categories;
using Models.ServiceModels.Product.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_Service.CustomerService
{
    public interface IProductCustomerService
    {
        IEnumerable<ProductCustomerViewModel> GetProducts(string? name, string[]? categories);
        ProductCustomerViewModel GetProduct(Guid productId);
        IEnumerable<ProductCustomerViewModel> GetProductsByCategories(string[] categories);
        IEnumerable<CategoryCustomerModel> GetCategories();       
    }
    #region implementation
    public class ProductCustomerService : IProductCustomerService
    {
        private readonly IProductCustomerFeature _customerFeature;
        public ProductCustomerService(IProductCustomerFeature productCustomerFeature)
        {
            _customerFeature = productCustomerFeature;
        }
        public IEnumerable<CategoryCustomerModel> GetCategories()
        {
            var result = _customerFeature.GetCategories();
            return result;
        }

        public ProductCustomerViewModel GetProduct(Guid productId)
        {
            return _customerFeature.GetProduct(productId);
        }

        public IEnumerable<ProductCustomerViewModel> GetProducts(string? name, string[]? categories)
        {
            var result = _customerFeature.GetProducts(name, categories);
            return result;
        }

        public IEnumerable<ProductCustomerViewModel> GetProductsByCategories(string[] categories)
        {
            var result = _customerFeature.GetProductsByCategories(categories);
            return result;
        }
    }
    #endregion
}
