using Admin.Application.Abstractions;
using Admin.Application.Models.Product;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.Infrastructure;
using Common.Infrastructure.Pagination;
using Common.Kernel.Dependencies;
using Common.Kernel.Extensions;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Services
{
    public class ProductService(AdminDbContext dbContext, 
        IMapper mapper) 
        : PaginationServiceBase<Product, PaginationRequest<ProductFilterModel>, ProductDto>(mapper, dbContext)
        , IProductService, IScoped
    {

        public async Task<Guid> CreateProduct(CreateProductDto request, CancellationToken ct)
        {
            var product = _mapper.Map<Product>(request);
            dbContext.Add(product);
            await dbContext.SaveChangesAsync(ct);
            return product.Id;
        }

        public async Task Delete(Guid id, CancellationToken ct)
        {
            await dbContext.Products.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
        }

        public async Task<ProductDto> GetDetail(GetDetailProductRequest request, CancellationToken ct)
        {
            ApplyInclude();
            Query = Query.QuerySlug<Product, Guid>(request.Value, QueryName);
            var product = await Query.FirstOrDefaultAsync();
            var response = _mapper.Map<ProductDto>(product);
            return response;
        }

        public async Task<PaginationResponse<ProductDto>> GetPaginationResponse(PaginationRequest<ProductFilterModel> request, CancellationToken ct)
        {
            var result = await ToPaginationResponse(request, QueryRequest);
            return result;
        }

        public async Task ModifyProduct(ModifyProductDto request, CancellationToken ct)
        {
            if (Guid.TryParse(request.Id, out var id))
            {
                var product = await dbContext.Products.Where(x => x.Id == id)
                    .Include(x => x.ProductCategories)
                    .FirstOrDefaultAsync();
                if (product is null)
                {
                    throw new CException($"Product not found for id: {id}");
                }
                _mapper.Map(request, product);
                #region Resolve product categories
                dbContext.Update(product);
                var category = await dbContext.Categories.Where(x => request.CategoryIds.Contains(x.Id))
                                                         .Include(x => x.Parent)
                                                         .ToListAsync();
                var requestAddCategories = category.SelectMany(x => x.Families().Select(c => c.Id));
                var toAdd = requestAddCategories.Except(product.ProductCategories.Select(x => x.CategoryId)).ToList();
                var toRemove = product.ProductCategories.ExceptBy(request.CategoryIds, x => x.CategoryId).Select(x => x.CategoryId).ToList();
                if (toRemove.Count > 0)
                {
                    await dbContext.Set<ProductCategory>()
                        .Where(pc => pc.ProductId == product.Id && toRemove.Contains(pc.CategoryId))
                        .ExecuteDeleteAsync(ct);
                }

                if (toAdd.Count > 0)
                {
                    var links = toAdd.Select(id => new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = id
                    });
                    dbContext.AddRange(links);
                } 
                #endregion
                await dbContext.SaveChangesAsync(ct);
            }
            else
            {
                throw new CException($"Invalid id: {request.Id}");
            }
        }
        private void ApplyInclude()
        {
            Query = Query.Include(x => x.ProductCategories)
                            .ThenInclude(x => x.Category)
                         .Include(x => x.ProductFavors)
                         .Include(x => x.ProductViews)
                         ;
        }
        private IQueryable<Product> QueryRequest(PaginationRequest<ProductFilterModel> request)
        {
            ApplyInclude();
            if (request.Filter is not null)
            {
                Query = Query.QuerySlug<Product, Guid>(request.Filter.Value, QueryName);
                Query = Query.QuerySlug<Product, Guid>(request.Filter.CategoryValue, QueryCategory);
            }
            return Query;
        }
        private static IQueryable<Product> QueryName(IQueryable<Product> query, string productName)
        {
            productName = productName.RemoveSpace().RemoveValue("-").RemoveAccent();
            query = query.Where(x =>
                            EF.Functions.ILike(
                                x.BareName,
                                EF.Functions.Unaccent($"%{productName}%")
                            ));
            return query;
        }

        private static IQueryable<Product> QueryCategory(IQueryable<Product> query, string categoryValue)
        {
            categoryValue = categoryValue.RemoveSpace().RemoveValue("-").RemoveAccent();
            query = query.Where(x => x.ProductCategories.Where(pc =>
                            EF.Functions.ILike(
                                pc.Category.BareName,
                                EF.Functions.Unaccent($"%{categoryValue}%")
                            )).Count() > 0);
            return query;
        }
    }
}
