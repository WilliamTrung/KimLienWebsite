using KL_Core;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KL_Repository.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private bool disposedValue;

        internal KimLienContext _context; 
        internal DbSet<TEntity> _entities;

        public GenericRepository(KimLienContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }
        public virtual void Add(TEntity entity)
        {
            if(entity is IAuditEntity)
            {
                ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow.AddHours(7);
                ((IAuditEntity)entity).LastModifiedDate = DateTime.UtcNow.AddHours(7);
            }
            _entities.Add(entity);
        }
                                                                                                
        public virtual void Delete(TEntity entity)
        {
            if(entity is IDeleteEntity)
            {
                ((IDeleteEntity)entity).IsDeleted = true;
            } else
            {
                _entities.Remove(entity);
            }            
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params string[] includeProperties)
        {
            IQueryable<TEntity> query = _entities;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query);
                //return query.OrderBy(orderBy);
            }
            else
            {
                return query;
            }
        }

        public virtual TEntity? GetFirst(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties)
        {
            IQueryable<TEntity> query = _entities;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault();
        }

        public virtual void Update(TEntity entity)
        {
            if (entity is IAuditEntity)
            {
                ((IAuditEntity)entity).LastModifiedDate = DateTime.UtcNow.AddHours(7);
            }
            _entities.Update(entity);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();                    
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
