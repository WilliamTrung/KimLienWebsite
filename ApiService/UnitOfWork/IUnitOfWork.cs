using ApiService.DTOs;
using AppRepository;
using AppRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<AppCore.Entities.Category> CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        GenericRepository<AppCore.Entities.Role> RoleRepository { get; }
        GenericRepository<AppCore.Entities.User> UserRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        void Save();
    }
}
