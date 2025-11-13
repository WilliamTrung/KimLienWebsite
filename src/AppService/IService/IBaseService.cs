using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AppService.Paging;

namespace AppService.IService
{
    public interface IBaseService<TEntity, TDto>
    where TEntity : class
    where TDto : class
    {
        public Task<TDto> Create(TDto dto);
        public Task<TDto> Update(Expression<Func<TEntity, bool>> filter, TDto dto);
        public Task<bool> Delete(Expression<Func<TEntity, bool>> filter, TDto dto);
        public Task<IEnumerable<TDto>> GetDTOs(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null, PagingRequest? paging = null);
        public void DisableSelfReference(ref TEntity entity);
    }
}
