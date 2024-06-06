using AutoMapper;
using KL_Repository.UnitOfWork;
using Models.Entities;
using Models.ServiceModels.Categories;
using Models.ServiceModels.Categories.Operation;
using Models.ServiceModels.Product.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KL_ProductCustomerFeature
{
    public interface IProductCustomerFeature
    {
        IEnumerable<ProductCustomerViewModel> GetProducts(string? name, string[]? categories);
        IEnumerable<ProductCustomerViewModel> GetProductsByCategories(string[] categories);
        Task<ProductCustomerViewModel> GetProductAsync(Guid productId);
        Task ViewCountIncrement(Product? product);
        IEnumerable<CategoryCustomerModel> GetCategories();       
    }
    #region implementation
    public class ProductCustomerFeature : IProductCustomerFeature
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductCustomerFeature(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<CategoryCustomerModel> GetCategories()
        {
            ////init result
            //List<CategoryCustomerModel> result = new List<CategoryCustomerModel>();
            ////get parent
            //var categories = _uow.CategoryRepository.Get().ToList();
            //var groups = categories.GroupBy(c => c.ParentId);
            //var parents = groups.Where(c => c.Key == null);
            //foreach (var category in parents)
            //{
            //    if (category == null)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        var model = _mapper.Map<CategoryCustomerModel>(category);
            //        var children = groups.Where(c => c.Key == category.Key);
            //        var childrenModels = _mapper.Map<List<CategoryCustomerModel>>(children);
            //        model.Children = childrenModels;
            //        result.Add(model);
            //    }
            //}            
            //return result;
            //init result
            List<CategoryCustomerModel> result = new List<CategoryCustomerModel>();
            //get parent
            var categories = _uow.CategoryRepository.Get().ToList();
            var groups = categories.GroupBy(c => c.ParentId);
            var parents = groups.Single(c => c.Key == null);
            foreach (var category in parents)
            {
                if (category == null)
                {
                    continue;
                }
                else
                {
                    var model = _mapper.Map<CategoryCustomerModel>(category);
                    var children = groups.SingleOrDefault(c => c.Key == category.Id);
                    if (children != null)
                    {
                        var childrenModels = _mapper.Map<List<CategoryCustomerModel>>(children);
                        model.Children = childrenModels;
                    }
                    result.Add(model);
                }
            }
            return result;
        }

        public async Task<ProductCustomerViewModel> GetProductAsync(Guid productId)
        {
            var product = _uow.ProductRepository.GetFirst(c => c.Id == productId, nameof(Product.ProductCategories), $"{nameof(Product.ProductCategories)}.{nameof(ProductCategory.Category)}");
            var result = _mapper.Map<ProductCustomerViewModel>(product);
            await ViewCountIncrement(product);
            return result;
        }

        public IEnumerable<ProductCustomerViewModel> GetProducts(string? name, string[]? categories)
        {
            Expression<Func<Product, bool>>? filter = null;
            if(name != null)
            {
                //append name searching for filter expression
                Expression<Func<Product, bool>> nameFilter = product => product.Name.Contains(name);
                filter = filter == null ? nameFilter : Expression.Lambda<Func<Product, bool>>(Expression.AndAlso(filter.Body, nameFilter.Body), filter.Parameters);
            }
           
            var products = _uow.ProductRepository.Get(filter, c => c.OrderBy(c => c.Name), nameof(Product.ProductCategories), $"{nameof(Product.ProductCategories)}.{nameof(ProductCategory.Category)}").ToList();
            if (categories != null)
            {
                products = products.Where(product => product.ProductCategories.Any(c => categories.Any(e => c.Category.Name.ToLower().Contains(e.ToLower())))).ToList();
            }
            var result = _mapper.Map<List<ProductCustomerViewModel>>(products);
            return result;
        }

        public IEnumerable<ProductCustomerViewModel> GetProductsByCategories(string[] categories)
        {
            throw new NotImplementedException();
        }

        public async Task ViewCountIncrement(Product? product)
        {
            if(product != null)
            {
                product.ViewCount++;
                _uow.ProductRepository.Update(product);
                await _uow.SaveAsync();
            }
        }
    }
    #endregion
}
