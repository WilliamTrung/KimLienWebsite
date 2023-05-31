using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        //C
        public void Create(TEntity entity);
        //R
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string ? includeProperties = null);
        public TEntity? GetById(params object?[]? keyValues);
        //U
        public void Update(Expression<Func<TEntity, bool>> filter, TEntity entity);
        public void Update(TEntity entity);
        //D
        public void Delete(TEntity entity);
    }
}
