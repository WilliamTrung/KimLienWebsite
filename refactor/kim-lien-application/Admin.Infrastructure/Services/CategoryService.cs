using Admin.Application.Abstractions;
using Admin.Application.Models.Category;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.Infrastructure;
using Common.Infrastructure.Pagination;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Admin.Infrastructure.Services
{
    public class CategoryService(AdminDbContext dbContext,
        IMapper mapper)
        : PaginationServiceBase<Category, PaginationRequest<CategoryFilterModel>, CategoryDto>(mapper, dbContext)
        , ICategoryService
    {
        public async Task<Guid> Create(CreateCategoryDto request, CancellationToken ct)
        {
            var category = _mapper.Map<Category>(request);
            dbContext.Add(category);
            await dbContext.SaveChangesAsync(ct);
            return category.Id;
        }

        public async Task Delete(Guid id, CancellationToken ct)
        {
            await dbContext.Categories.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
        }

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

        public async Task Update(ModifyCategoryDto request, CancellationToken ct)
        {
            var category = await dbContext.Categories.Where(x => x.Id == request.Id)
                                                     .Include(x => x.Parent)
                                                     .FirstOrDefaultAsync();
            if (category is null)
            {
                throw new Exception($"Category not found for id: {request.Id}");
            }
            _mapper.Map(request, category);
            dbContext.Update(category);
            await dbContext.SaveChangesAsync(ct);
        }
        
        private void ApplyInclude()
        {
            Query = Query.Include(x => x.Parent);
        }
        private Expression<Func<Category, bool>> QueryRequest(PaginationRequest<CategoryFilterModel> request)
        {
            ApplyInclude();
            var query = PredicateBuilder.New<Category>();
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
                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    var filterName = PredicateBuilder.New<Category>();
                    if (!string.IsNullOrWhiteSpace(request.Filter.Name) && Guid.TryParse(request.Filter.Name, out var id))
                    {
                        filterName = filterName.Or(x => x.Id == id);
                    }
                    filterName = filterName.Or(x => EF.Functions.Like(EF.Functions.Unaccent(x.Name).ToLower().Trim(),
                                                EF.Functions.Unaccent(filter.Name).ToLower().Trim())
                                        );
                    query = query.And(filterName);
                } 
            }
            return query;
        }
        private static IQueryable<Category> QueryName(IQueryable<Category> query, string categoryName)
        {
            query = query.Where(x =>
                            EF.Functions.ILike(
                                EF.Functions.Unaccent(x.Name).ToLower().Trim(),
                                EF.Functions.Unaccent($"%{categoryName}%")
                            ));
            return query;
        }
    }
}
