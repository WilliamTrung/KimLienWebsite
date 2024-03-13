using AutoMapper;
using KL_Repository.UnitOfWork;
using Models.Entities;
using Models.ServiceModels.Categories;
using Models.ServiceModels.Categories.Operation;
using Models.ServiceModels.Product.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_ManagementFeature
{
    public interface ICategoryManagementFeature
    {
        IEnumerable<CategoryViewModel> GetCategories();
        IEnumerable<CategoryViewModel> GetChildren(Guid id);
        CategoryViewModel GetCategoryById(Guid categoryId);
        Task<Guid> AddCategory(CategoryAddModel model);
        Task ModifyCategory(CategoryModifyModel model);
        Task ToggleCategoryStatus(Guid categoryId);
        Task DeleteCategory(Guid categoryId);
    }
    public class CategoryManagementFeature : ICategoryManagementFeature
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CategoryManagementFeature(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Guid> AddCategory(CategoryAddModel model)
        {
            model.Name = model.Name.Trim();
            var isValidName = CheckNameDuplicated(model.Name, model.ParentId);
            if (!isValidName)
            {
                throw new InvalidDataException("DUPLICATED");
            }
            var entity = _mapper.Map<Category>(model);
            _uow.CategoryRepository.Add(entity);
            await _uow.SaveAsync();
            return entity.Id;
        }

        public async Task DeleteCategory(Guid categoryId)
        {
            var category = _uow.CategoryRepository.GetFirst(c => c.Id ==  categoryId);  
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found for id: " + categoryId);
            }
            try
            {                
                _uow.CategoryRepository.Delete(category);
                await _uow.SaveAsync();
            } catch (Exception ex)
            {
                throw new InvalidOperationException("This category is within another product: " + ex.Message);
            }
            
        }

        public IEnumerable<CategoryViewModel> GetCategories()
        {
            //init result
            List<CategoryViewModel> result = new List<CategoryViewModel>();
            //get parent
            var categories = _uow.CategoryRepository.Get();
            var groups = categories.GroupBy(c => c.ParentId);
            var parents = groups.Where(c => c.Key == null);
            foreach (var category in parents)
            {
                if (category == null)
                {
                    continue;
                }
                else
                {
                    var model = _mapper.Map<CategoryViewModel>(category);
                    var children = groups.Where(c => c.Key == category.Key);
                    var childrenModels = _mapper.Map<List<CategoryViewModel>>(children);
                    model.Children = childrenModels;
                    result.Add(model);
                }
            }          
            return result;
        }
        public IEnumerable<CategoryViewModel> GetChildren(Guid id)
        {
            var categories = _uow.CategoryRepository.Get(c => c.ParentId == id);
            var result = _mapper.Map<List<CategoryViewModel>>(categories);
            return result;
        }
        public CategoryViewModel GetCategoryById(Guid categoryId)
        {
            var find = _uow.CategoryRepository.GetFirst(c  => c.Id == categoryId);
            if(find == null)
            {
                throw new KeyNotFoundException("Category not found for id: " + categoryId);
            }
            var model = _mapper.Map<CategoryViewModel>(find);
            if (find.ParentId != null)
            {
                var children = _uow.CategoryRepository.Get(c => c.ParentId == categoryId);
                var models = _mapper.Map<List<CategoryViewModel>>(children);
                model.Children = models;
            }
            return model;
        }

        public async Task ModifyCategory(CategoryModifyModel model)
        {
            var category = _uow.CategoryRepository.GetFirst(c => c.Id == model.Id);
            if(category == null)
            {
                throw new KeyNotFoundException("Category not found for id: " + model.Id);
            }
            var isValidName = CheckNameDuplicated(model.Name, category.ParentId);
            if (!isValidName)
            {
                throw new InvalidDataException("DUPLICATED");
            }
            category.Name = model.Name;            
            _uow.CategoryRepository.Update(category);
            await _uow.SaveAsync();
        }

        public async Task ToggleCategoryStatus(Guid categoryId)
        {
            var category = _uow.CategoryRepository.GetFirst(c => c.Id == categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found for id: " + categoryId);
            }
            category.IsDeleted = !category.IsDeleted;
            //if(category.ParentId == null)
            //{
            //    var children = _uow.CategoryRepository.Get(c => c.ParentId == category.Id);
            //    foreach (var child in children)
            //    {
            //        child.IsDeleted = category.
            //    }
            //}
            _uow.CategoryRepository.Update(category);
            await _uow.SaveAsync();
        }
        private bool CheckNameDuplicated(string categoryName, Guid? parentId)
        {
            var find = _uow.CategoryRepository.Get(c => c.Name == categoryName && c.ParentId == parentId);
            if (find != null && find.Count() > 0) {
                return false;
            }
            return true;
        }
    }
}
