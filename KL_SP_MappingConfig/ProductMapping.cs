using AutoMapper;
using Models.Entities;
using Models.ServiceModels.Categories;
using Models.ServiceModels.Product.Operation;
using Models.ServiceModels.Product.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_SP_MappingConfig
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            Map_Product_ProductAdminViewModel();
            Map_Product_ProductCustomerViewModel();
            Map_ProductAddModel_Product();
            Map_ProductCategory_CategoryAdminViewModel();
        }
        private void Map_Product_ProductCustomerViewModel()
        {
            CreateMap<Product, ProductCustomerViewModel>()
                .AfterMap<Map_Product_ProductCustomerViewModel>();
        }
        private void Map_ProductAddModel_Product()
        {
            CreateMap<ProductAddModel, Product>().AfterMap<Map_ProductAddModel_Product>();         
        }
        private void Map_Product_ProductAdminViewModel()
        {
            CreateMap<Product, ProductAdminViewModel>()
                .ForMember(c => c.Pictures, o =>
                {
                    o.Ignore();
                })
                .AfterMap<Map_Product_ProductAdminViewModel>();
        }
        private void Map_ProductCategory_CategoryAdminViewModel()
        {
            CreateMap<ProductCategory, CategoryAdminViewModel>()
                .AfterMap<Map_ProductCategory_CategoryAdminViewModel>();
        }
    }    
    public class Map_Product_ProductCustomerViewModel : IMappingAction<Product, ProductCustomerViewModel>
    {
        public void Process(Product source, ProductCustomerViewModel destination, ResolutionContext context)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.LastModifiedDate = source.LastModifiedDate;
            var pictures = source.Pictures.Split(',').ToList();
            destination.Pictures = pictures;
            destination.ViewCount = source.ViewCount;
            destination.Categories = source.ProductCategories.Select(c => c.Category.Name).ToList();
        }
    }
    public class Map_ProductAddModel_Product : IMappingAction<ProductAddModel, Product>
    {
        public void Process(ProductAddModel source, Product destination, ResolutionContext context)
        {
            destination.Name = source.Name;
            string pictures = "";
            foreach (var picture in source.Pictures)
            {
                pictures += picture + ",";
            }
            destination.Pictures = pictures;
        }
    }
    public class Map_Product_ProductAdminViewModel : IMappingAction<Product, ProductAdminViewModel>
    {
        private readonly IMapper _mapper;
        public Map_Product_ProductAdminViewModel(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void Process(Product source, ProductAdminViewModel destination, ResolutionContext context)
        {            
            destination.CreatedDate = source.CreatedDate;
            destination.LastModifiedDate= source.LastModifiedDate;
            destination.Name = source.Name;
            destination.IsDeleted = source.IsDeleted;
            destination.ViewCount= source.ViewCount;
            var list = source.Pictures.Split(",").ToList();
            list.Remove(string.Empty);
            destination.Pictures = list;
            destination.Categories = new List<CategoryAdminViewModel>();
            //var categories = _mapper.Map<List<CategoryAdminViewModel>>(source.ProductCategories);
            var groups = source.ProductCategories.GroupBy(c => c.Category.ParentId);
            foreach (var group in groups)
            {
                //init parent
                if(group.Key == null)
                {
                    foreach (var category in group)
                    {
                        var categoryModel = _mapper.Map<CategoryAdminViewModel>(category);
                        destination.Categories.Add(categoryModel);
                    }
                } else
                {
                    //add children
                    var parent = destination.Categories.Single(c => c.Id == group.Key);
                    if(parent.Children == null)
                        parent.Children = new List<CategoryAdminViewModel>();
                    foreach (var category in group)
                    {
                        var childModel = _mapper.Map<CategoryAdminViewModel>(category);
                        parent.Children.Add(childModel);
                    }
                }
                
            }
        }
    }
    public class Map_ProductCategory_CategoryAdminViewModel : IMappingAction<ProductCategory, CategoryAdminViewModel>
    {
        public void Process(ProductCategory source, CategoryAdminViewModel destination, ResolutionContext context)
        {
            destination.Id = source.Category.Id;
            destination.IsDeleted = source.Category.IsDeleted;
            destination.Name = source.Category.Name;
        }
    }
}
