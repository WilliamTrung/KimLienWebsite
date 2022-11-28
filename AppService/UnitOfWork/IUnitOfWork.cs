using AppService.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryService CategoryService { get; }
        IProductService ProductService { get; }
        IRoleService RoleService { get; }
        IUserService UserService { get; }
    }
}
