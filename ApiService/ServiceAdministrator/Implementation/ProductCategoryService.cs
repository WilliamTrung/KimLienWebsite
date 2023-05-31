using ApiService.Azure;
using ApiService.DTOs;
using ApiService.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator.Implementation
{
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService) : base(unitOfWork, mapper, azureService)
        {
        }
        /// <summary>
        /// Add Category to a product
        /// <para>Throw DuplicateWaitObjectException: This product category has already existed</para>
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="categoryId"></param>
        /// <exception cref="DuplicateWaitObjectException"></exception>
        public void AddCategory(Guid productId, Guid categoryId)
        {
            using (var pcRepos = _unitOfWork.ProductCategoryRepository)
            {
                var check = pcRepos.Get(filter: pc => pc.CategoryId == categoryId && pc.ProductId == productId).FirstOrDefault();
                if (check != null)
                {
                    if(check.IsDeleted)
                    {
                        check.IsDeleted = false;
                        pcRepos.Update(check);
                    } else
                    {
                        throw new DuplicateWaitObjectException();
                    }
                    
                } else
                {
                    var pc = new ProductCategory()
                    {
                        CategoryId= categoryId,
                        ProductId= productId                        
                    };
                    pcRepos.Create(_mapper.Map<AppCore.Entities.ProductCategory>(pc));
                }
                _unitOfWork.Save();
            }
        }
        /// <summary>
        /// Delete passed category and all its relevant children
        /// <para>Throw NullReferenceException: No category found for this product</para>
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="categoryId"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void RemoveCategory(Guid productId, Guid categoryId)
        {
            using (var pcRepos = _unitOfWork.ProductCategoryRepository)
            {
                var check = pcRepos.Get(filter: pc => pc.CategoryId == categoryId && pc.ProductId == productId).FirstOrDefault();
                if (check != null)
                {
                    pcRepos.DeleteByCategory(categoryId);
                    _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }                
            }
        }
    }
}
