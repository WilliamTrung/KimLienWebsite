using AppCore;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRepository;
using ApiService.DTOs;
using AppRepository.Repository;
using AppRepository.Repository.Implementation;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ApiService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private SqlContext _context;

        private GenericRepository<AppCore.Entities.Category>? _categoryRepository;
        private IProductRepository? _productRepository;
        private GenericRepository<AppCore.Entities.Role>? _roleRepository;
        private GenericRepository<AppCore.Entities.User>? _userRepository;
        private IProductCategoryRepository? _productCategoryRepository;
        public UnitOfWork(SqlContext context)
        {
            _context = context;
        }

        public GenericRepository<AppCore.Entities.Category> CategoryRepository
        {
            get
            {
                if(_categoryRepository == null)
                {
                    _categoryRepository = new GenericRepository<AppCore.Entities.Category>(_context);                   
                }
                return _categoryRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {                    
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        public GenericRepository<AppCore.Entities.Role> RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new GenericRepository<AppCore.Entities.Role>(_context);
                }
                return _roleRepository;
            }
        }

        public GenericRepository<AppCore.Entities.User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<AppCore.Entities.User>(_context);
                }
                return _userRepository;
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                if (_productCategoryRepository == null)
                {
                    _productCategoryRepository = new ProductCategoryRepository(_context);
                }
                return _productCategoryRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
