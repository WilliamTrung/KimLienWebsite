using AutoMapper;
using Models.Entities;
using Models.ServiceModels.Categories;
using Models.ServiceModels.Categories.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_SP_MappingConfig
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            Map_Category_CategoryCustomerModel();
            Map_Category_CategoryViewModel();
            Map_CategoryAddModel_Category();
        }
        private void Map_CategoryAddModel_Category()
        {
            CreateMap<CategoryAddModel, Category>()
                .AfterMap<MapCategoryAddModelCategory>();
        }
        private void Map_Category_CategoryViewModel()
        {
            CreateMap<Category, CategoryViewModel>()
                .AfterMap<MapCategoryCategoryViewModel>();
        }
        private void Map_Category_CategoryCustomerModel()
        {
            CreateMap<Category, CategoryCustomerModel>()
                .AfterMap<MapCategoryCategoryCustomerModel>();
        }
    }
    public class MapCategoryCategoryCustomerModel : IMappingAction<Category, CategoryCustomerModel>
    {
        public void Process(Category source, CategoryCustomerModel destination, ResolutionContext context)
        {
            destination.Name = source.Name;
        }
    }
    public class MapCategoryCategoryViewModel : IMappingAction<Category, CategoryViewModel>
    {
        public void Process(Category source, CategoryViewModel destination, ResolutionContext context)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.IsDeleted = source.IsDeleted;
        }
    }
    public class MapCategoryAddModelCategory : IMappingAction<CategoryAddModel, Category>
    {
        public void Process(CategoryAddModel source, Category destination, ResolutionContext context)
        {
            destination.Name = source.Name;
            destination.ParentId = source.ParentId;
            destination.IsDeleted = false;
        }
    }
}
