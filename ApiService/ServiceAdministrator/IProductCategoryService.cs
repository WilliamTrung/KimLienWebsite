using ApiService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator
{
    public interface IProductCategoryService
    {
        void AddCategory(Guid productId, Guid categoryId);
        void RemoveCategory(Guid productId, Guid categoryId);
    }
}
