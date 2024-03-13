using KL_ManagementFeature;
using Models.ServiceModels.Categories.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_Service.CategoryService
{
    public interface ICategoryManagementService
    {
        IEnumerable<CategoryViewModel> GetCategories();
        IEnumerable<CategoryViewModel> GetChildren(Guid id);
        CategoryViewModel GetCategory(Guid id);
        Task<Guid> AddCategory(CategoryAddModel model);
        Task ModifyCategory(CategoryModifyModel model);
        Task ToggleCategoryStatus(Guid categoryId);
        Task DeleteCategory(Guid categoryId);
    }
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly ICategoryManagementFeature _categoryManageFeature;
        public CategoryManagementService(ICategoryManagementFeature categoryManagementFeature)
        {
            _categoryManageFeature = categoryManagementFeature;
        }
        public Task<Guid> AddCategory(CategoryAddModel model)
        {
            return _categoryManageFeature.AddCategory(model);
        }

        public Task DeleteCategory(Guid categoryId)
        {
            return _categoryManageFeature.DeleteCategory(categoryId);
        }

        public IEnumerable<CategoryViewModel> GetCategories()
        {
            return _categoryManageFeature.GetCategories();
        }

        public CategoryViewModel GetCategory(Guid id)
        {
            return _categoryManageFeature.GetCategoryById(id);
        }

        public IEnumerable<CategoryViewModel> GetChildren(Guid id)
        {
            return _categoryManageFeature.GetChildren(id);
        }

        public Task ModifyCategory(CategoryModifyModel model)
        {
            return _categoryManageFeature.ModifyCategory(model);
        }

        public Task ToggleCategoryStatus(Guid categoryId)
        {
            return _categoryManageFeature.ToggleCategoryStatus(categoryId);
        }
    }
}
