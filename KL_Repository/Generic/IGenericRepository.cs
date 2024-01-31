using System.Linq.Expressions;

namespace KL_Repository.Generic
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties);
        Task<TEntity?> GetFirst(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties);
    }
}
