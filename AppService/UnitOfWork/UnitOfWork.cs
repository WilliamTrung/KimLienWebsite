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

        public ICategoryService CategoryService { get; private set; } = null!;

        public IProductService ProductService { get; private set; } = null!;

        public IRoleService RoleService { get; private set; } = null!;

        public IUserService UserService { get; private set; } = null!;

        public IProductCategoryService ProductCategoryService { get; private set; } = null!;

        public UnitOfWork(SqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
            InitServices();
        }

        void InitServices()
        {
            CategoryService = new CategoryService(_context,_mapper);
            ProductService = new ProductService(_context, _mapper);
            ProductCategoryService = new ProductCategoryService(_context, _mapper);
            RoleService = new RoleService(_context, _mapper);
            UserService = new UserService(_context, _mapper);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
