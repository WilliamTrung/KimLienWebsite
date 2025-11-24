using System.Linq.Expressions;
using Admin.Application.Abstractions;
using Admin.Application.Models.Category;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.Infrastructure;
using Common.Infrastructure.DbContext;
using Common.Infrastructure.Pagination;
using Common.Kernel.Dependencies;
using Common.Kernel.Extensions;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Admin.Infrastructure.Services
{
    public class CategoryService(AdminDbContext dbContext,
        ILogger<CategoryService> logger,
        IMapper mapper)
        : PaginationServiceBase<Category, PaginationRequest<CategoryFilterModel>, CategoryDto>(mapper, dbContext)
        , ICategoryService, IScoped
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
            try
            {
                await dbContext.ExecuteTransactionAsync(request, ModifyCategory, ct);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error modify category. Request: {@Request}", request);
                throw;
            }
        }
        private async Task ModifyCategory(ModifyCategoryDto request, CancellationToken ct)
        {
            var category = await dbContext.Categories.Where(x => x.Id == request.Id)
                                                     .Include(x => x.Parent)
                                                     .FirstOrDefaultAsync();
            if (category is null)
            {
                throw new CException($"Category not found for id: {request.Id}");
            }
            if (request.ParentId != category.ParentId)
            {
                //get all the products attach to current category
                var productsWithCategory = await dbContext.ProductCategories
                                .Where(p => p.CategoryId == category.Id).ToListAsync();
                var productIds = productsWithCategory.Select(p => p.ProductId).ToList();
                List<Guid> categories = new List<Guid>();
                if (category.ParentId is not null)
                {
                    //get all the parent of current category
                    var categoriesParentChange = await dbContext.Categories.FromSqlRaw(CategoryExtension.QueryParents, new { childId = category.Id }).ToListAsync();
                    categories = categoriesParentChange.Select(c => c.Id).ToList();
                }
                else if (request.ParentId is not null) // case current category has no parent, update new parent
                {
                    //assign new parentId -> map all parents to all attached products
                    //get all the parent of new parent category
                    var categoriesParentChange = await dbContext.Categories.FromSqlRaw(CategoryExtension.QueryParents, new { childId = request.ParentId }).ToListAsync();
                    categories = categoriesParentChange.Select(c => c.Id).ToList();
                    categories.Add(request.ParentId.Value);
                }

                if (categories.Any() && productIds.Any())
                {
                    await dbContext.ProductCategories.Where(x =>
                                    //here I want to delete all product categories that belong to the category being modified and its parents
                                    productIds.Contains(x.ProductId) && categories.Contains(x.CategoryId)
                                ).ExecuteDeleteAsync();
                }
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
