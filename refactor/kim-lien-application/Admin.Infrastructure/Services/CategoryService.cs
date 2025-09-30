using Admin.Application.Abstractions;
using Admin.Application.Models.Category;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.Infrastructure.Pagination;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using Microsoft.EntityFrameworkCore;

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
            if (!string.IsNullOrWhiteSpace(request.Id) && Guid.TryParse(request.Id, out var id))
            {
                Query = Query.Where(x => x.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                Query = Query.Where(x => EF.Functions.Like(EF.Functions.Unaccent(x.Name).ToLower().Trim(),
                                            EF.Functions.Unaccent(request.Name).ToLower().Trim())
                                    );
            }
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
        private IQueryable<Category> QueryRequest(PaginationRequest<CategoryFilterModel> request)
        {
            ApplyInclude(); 
            var query = Query;
            if (request.Filter is not null)
            {
                var filter = request.Filter;
                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    query = query.Where(x => EF.Functions.Like(EF.Functions.Unaccent(x.Name).ToLower().Trim(),
                                                EF.Functions.Unaccent(filter.Name).ToLower().Trim())
                                        );
                } 
            }
            return query;
        }
    }
}
