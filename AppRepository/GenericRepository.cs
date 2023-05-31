using AppCore;
using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal SqlContext _context;
        internal DbSet<TEntity> _dbSet;
        private bool disposed = false;
        public GenericRepository(SqlContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public virtual void Create(TEntity entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");
                if(entity is IAuditEntity)
                {
                    ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
                    ((IAuditEntity)entity).ModifiedDate = DateTime.UtcNow;
                }
                if(entity is IDeleteEntity)
                {
                    ((IDeleteEntity)entity).IsDeleted = false;
                }
                _dbSet.Add(entity);
            }
            catch (Exception)
            {
                throw new Exception("GenericRepository Add Failed");
            }
        }

        public virtual void Delete(TEntity entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");
                if (entity is IDeleteEntity)
                {
                    ((IDeleteEntity)entity).IsDeleted = true;
                    _dbSet.Update(entity);
                }
                else
                {
                    _dbSet.Remove(entity);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string ? includeProperties = null)
        {
            var query = _dbSet.AsQueryable();
            if (query.FirstOrDefault() != null && query.FirstOrDefault() is Product)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                query = query.OrderByDescending(p => (p as Product).ModifiedDate);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            
            if (includeProperties != null)
            {
                foreach (string property in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.AsEnumerable();
        }


        public virtual void Update(TEntity entity)
        {
            try
            {
                if (entity is IAuditEntity)
                {
                    ((IAuditEntity)entity).ModifiedDate = DateTime.UtcNow;
                }
                _context.Entry(entity).State = EntityState.Modified;;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public virtual void Update(Expression<Func<TEntity, bool>> filter, TEntity entity)
        {
            var find = Get(filter);
            var found = find.FirstOrDefault();
            if (found != null)
            {
                if (entity is IAuditEntity)
                {
                    ((IAuditEntity)entity).ModifiedDate = DateTime.UtcNow;
                }
                _context.Entry<TEntity>(found).CurrentValues.SetValues(entity);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
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

        public virtual TEntity? GetById(params object?[]? keyValues)
        {
            return _dbSet.Find(keyValues);
        }
    }
}
