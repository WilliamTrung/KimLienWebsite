using AutoMapper;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Infrastructure.Pagination
{
    public abstract class PaginationServiceBase<TEntity, TResponse>(IMapper mapper, DbContext dbContext)
        where TEntity : class
        where TResponse : class
    {
        protected readonly IMapper _mapper = mapper;
        protected DbSet<TEntity> Entities => dbContext.Set<TEntity>();
        private IQueryable<TEntity>? _query { get; set; }
        protected IQueryable<TEntity> Query
        {
            get
            {
                if(_query is null)
                {
                    _query = Entities.AsQueryable();
                }
                return _query.AsNoTracking();
            }
            set { _query = value; }
        }
        protected virtual async Task<PaginationResponse<TResponse>> ToPaginationResponse(PaginationRequest request)
        {
            var countByFilter = await Query.CountAsync();
            var paginationQuery = Query.ToPaginationQuery(request.PageIndex, request.PageSize);
            Query = paginationQuery.Query;
            if (request.SortField?.Any() ?? false)
            {
                Query = Query.ApplySorting(request.SortField, request.Asc ?? false); 
            }
            var result = await Query.ToListAsync();
            var responseData = _mapper.Map<List<TResponse>>(result);
            var response = new PaginationResponse<TResponse>
            {
                Results = responseData,
                CurrentPage = paginationQuery.PageIndex,
                PageSize = paginationQuery.PageSize,
                RowCount = countByFilter,
            };
            return response;
        }
        protected virtual async Task<PaginationResponse<TResponse>> ToPaginationResponse(PaginationRequest request, IQueryable<TEntity> query)
        {
            Query = query;
            return await ToPaginationResponse(request);
        }
    }
    public abstract class PaginationServiceBase<TEntity, TRequest, TResponse>(IMapper mapper, DbContext dbContext) 
        : PaginationServiceBase<TEntity, TResponse>(mapper, dbContext)
        where TEntity : class
        where TResponse : class
        where TRequest : PaginationRequest
    {
        protected virtual async Task<PaginationResponse<TResponse>> ToPaginationResponse(TRequest request, Func<TRequest, IQueryable<TEntity>> queryFunc)
            
        {
            Query = queryFunc.Invoke(request);
            return await ToPaginationResponse(request);
        }
        protected virtual async Task<PaginationResponse<TResponse>> ToPaginationResponse(TRequest request, Func<TRequest, Expression<Func<TEntity, bool>>> queryFunc)

        {
            Query = Query.Where(queryFunc.Invoke(request));
            return await ToPaginationResponse(request);
        }
    }
}
