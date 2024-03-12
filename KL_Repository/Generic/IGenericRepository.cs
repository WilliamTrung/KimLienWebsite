using System.Linq.Expressions;

namespace KL_Repository.Generic
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params string[] includeProperties);
        TEntity? GetFirst(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties);
    }
}
