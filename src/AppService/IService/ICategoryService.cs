using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.IService
{
    public interface ICategoryService : IBaseService<Category, DTOs.Category>
    {
    }
}
