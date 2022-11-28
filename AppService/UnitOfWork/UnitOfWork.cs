using AppCore;
using AppService.IService;
using AppService.Service;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private SqlContext _context;
        private IMapper _mapper;


        public UnitOfWork(SqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }
        public ICategoryService CategoryService
        {
            private set { }
            get
            {
                if (CategoryService == null)
                    CategoryService = new CategoryService(_context,_mapper);
                return CategoryService;
            }
        }
        public IProductService ProductService
        {
            private set { }
            get
            {
                if (ProductService == null)
                    ProductService = new ProductService(_context, _mapper);
                return ProductService;
            }
        }
        public IRoleService RoleService
        {
            private set { }
            get
            {
                if (RoleService == null)
                    RoleService = new RoleService(_context, _mapper);
                return RoleService;
            }
        }
        public IUserService UserService
        {
            private set { }
            get
            {
                if (UserService == null)
                    UserService = new UserService(_context, _mapper);
                return UserService;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
