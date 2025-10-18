using AutoMapper;
using Client.Application.Abstractions;
using Client.Application.Models.Category;
using Client.Infrastructure.Data;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.Infrastructure;
using Common.Infrastructure.Pagination;
using Common.Kernel.Dependencies;
using Common.Kernel.Extensions;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Client.Infrastructure.Services
{
    public class CategoryService(ClientDbContext dbContext,
          ILogger<CategoryService> logger,
          IMapper mapper)
          : PaginationServiceBase<Category, PaginationRequest<CategoryFilterModel>, CategoryDto>(mapper, dbContext)
          , ICategoryService, IScoped
    {
        public async Task<CategoryDto> GetDetail(GetDetailCategoryRequest request, CancellationToken ct)
        {
            ApplyInclude();
            Query = Query.QuerySlug<Category, Guid>(request.Value, QueryName);
            var category = await Query.FirstOrDefaultAsync();
            var response = _mapper.Map<CategoryDto>(category);
            return response;
        }

        public async Task<PaginationResponse<CategoryDto>> GetPagination(PaginationRequest<CategoryFilterModel> request, CancellationToken ct)
        {
            return await ToPaginationResponse(request, QueryRequest);
        }

        private void ApplyInclude()
        {
            Query = Query.Include(x => x.Parent);
        }
        private Expression<Func<Category, bool>> QueryRequest(PaginationRequest<CategoryFilterModel> request)
        {
            ApplyInclude();
            var query = PredicateBuilder.New<Category>(x => true);
            if (request.Filter is not null)
            {
                var filter = request.Filter;
                if (!string.IsNullOrWhiteSpace(request.Filter.ParentId) && Guid.TryParse(request.Filter.ParentId, out var parentId))
                {
                    query = query.And(x => x.ParentId == parentId);
                }
                else
                {
                    query = query.And(x => x.ParentId == null);
                }
                query = query.And(QueryableExtension.BuildSlugQuery<Category, Guid>(filter.Value, BuildQueryName));
            }
            return query;
        }
        private static Expression<Func<Category, bool>> BuildQueryName(string categoryName)
        {
            var query = PredicateBuilder.New<Category>(x =>
                            EF.Functions.ILike(
                                EF.Functions.Unaccent(x.Name).ToLower().Trim(),
                                EF.Functions.Unaccent($"%{categoryName}%")
                            ));
            return query;
        }
        private static IQueryable<Category> QueryName(IQueryable<Category> query, string categoryName)
        {
            categoryName = categoryName.RemoveSpace().RemoveValue("-").RemoveAccent();
            query = query.Where(x =>
                            EF.Functions.ILike(
                                x.BareName,
                                EF.Functions.Unaccent($"%{categoryName}%")
                            ));
            return query;
        }
    }
}
