using AutoMapper;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Common.Infrastructure.Pagination
{
    public class PaginationServiceBase<TEntity, TResponse>(IMapper mapper, DbContext dbContext)
        where TEntity : class
        where TResponse : class
    {
        protected DbSet<TEntity> Entities => dbContext.Set<TEntity>();
        private IQueryable<TEntity>? _query { get; set; }
        protected IQueryable<TEntity> Query
        {
            get
            {
                if(_query is null)
                {
                    _query = Entities.AsQueryable().AsNoTracking();
                }
                return _query;
            }
            set { _query = value; }
        }
        public virtual async Task<PaginationResponse<TResponse>> ToPaginationResponse(PaginationRequest request)
        {
            var countByFilter = await Query.CountAsync();
            var paginationQuery = Query.ToPaginationQuery(request.PageIndex, request.PageSize);
            Query = paginationQuery.Query;
            if (request.SortField?.Any() ?? false)
            {
                Query = Query.ApplySorting(request.SortField, request.Asc ?? false); 
            }
            var result = await Query.ToListAsync();
            var responseData = mapper.Map<List<TResponse>>(result);
            var response = new PaginationResponse<TResponse>
            {
                Results = responseData,
                CurrentPage = paginationQuery.PageIndex,
                PageSize = paginationQuery.PageSize,
                RowCount = countByFilter,
            };
            return response;
        }
    }
}
