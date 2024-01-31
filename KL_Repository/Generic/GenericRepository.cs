using Models.Entities;
using System.Linq.Expressions;

namespace KL_Repository.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private bool disposedValue;

        //private DbSet<TEntity> _entities;

        public GenericRepository()
        {
            
        }
        public Task Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetFirst(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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
