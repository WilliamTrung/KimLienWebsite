using ApiService.Azure;
using ApiService.DTOs;
using ApiService.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator.Implementation
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService) : base(unitOfWork, mapper, azureService)
        {
        }

        /// <summary>
        /// Add new Category
        /// <para>Throw DuplicateNameException: Duplicated name on category</para>
        /// <para>Throw ArgumentException: Selected parent for this category is a child of another category</para>
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="DuplicateNameException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Add(DTOs.Category category)
        {
            using (var categoryRepos = _unitOfWork.CategoryRepository)
            {
                var checkDuplicated = categoryRepos.Get().Where(c => string.Equals(category.Name, c.Name, StringComparison.OrdinalIgnoreCase));
                if(checkDuplicated.Any())
                {
                    throw new DuplicateNameException();
                }
                var checkParent = categoryRepos.Get(filter: c => c.Id == category.ParentId).FirstOrDefault();
                if(checkParent != null && checkParent.ParentId != null)
                {
                    throw new ArgumentException("Selected parent category is a child!");
                }
                categoryRepos.Create(_mapper.Map<AppCore.Entities.Category>(category));
                _unitOfWork.Save();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var categoryRepos = _unitOfWork.CategoryRepository)
            {
                var category = categoryRepos.GetById(id);
                if(category == null)
                    throw new KeyNotFoundException();
                    
                await DeleteChildren(_mapper.Map<Category>(category));                
                _unitOfWork.Save();
            }
        }    
        private Task DeleteChildren(DTOs.Category category)
        {
            using (var categoryRepos = _unitOfWork.CategoryRepository)
            {
                if (category.ParentId == null)
                {
                    //is head --> delete children

                    {
                        var children = categoryRepos.Get(filter: c => c.ParentId == category.Id);
                        if (children != null)
                        {
                            using (var productCategoryRepos = _unitOfWork.ProductCategoryRepository)
                            {
                                foreach (var child in children)
                                {
                                    productCategoryRepos.DeleteByCategory(child.Id);
                                    DeleteChildren(_mapper.Map<Category>(child));
                                }
                            }
                        }

                    }
                }
                categoryRepos.Delete(_mapper.Map<AppCore.Entities.Category>(category));
            }
            return Task.CompletedTask;
        }
        public IEnumerable<DTOs.Category> GetCategories()
        {
            using(var categoryRepos = _unitOfWork.CategoryRepository)
            {
                return _mapper.Map<IEnumerable<Category>>(categoryRepos.Get(orderBy: c => c.OrderBy(c => c.Name)));
            }
        }
        /// <summary>
        /// Update new Category
        /// <para>Throw DuplicateNameException: Duplicated name on category</para>
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="DuplicateNameException"></exception>
        public void Update(DTOs.Category category)
        {
            using (var categoryRepos = _unitOfWork.CategoryRepository)
            {
                var checkDuplicated = categoryRepos.Get().Where(c => Extension.StringExtension.MinimalCompareString(category.Name, c.Name));
                if (checkDuplicated.Any())
                {
                    throw new DuplicateNameException();
                }
                categoryRepos.Update(_mapper.Map<AppCore.Entities.Category>(category));
                _unitOfWork.Save();
            }
        }
    }
}
