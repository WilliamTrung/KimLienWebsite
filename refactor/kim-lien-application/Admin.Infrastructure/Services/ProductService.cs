using Admin.Application.Abstractions;
using Admin.Application.Models.Product;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.Infrastructure.Pagination;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Services
{
    public class ProductService(AdminDbContext dbContext, 
        IMapper mapper) 
        : PaginationServiceBase<Product, PaginationRequest<ProductFilterModel>, ProductDto>(mapper, dbContext)
        , IProductService
    {

        public async Task<Guid> CreateProduct(CreateProductDto request, CancellationToken ct)
        {
            var product = _mapper.Map<Product>(request);
            dbContext.Add(product);
            await dbContext.SaveChangesAsync(ct);
            return product.Id;
        }

        public async Task DeleteProduct(Guid id, CancellationToken ct)
        {
            await dbContext.Products.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
        }

        public async Task<ProductDto> GetDetail(GetDetailProductRequest request, CancellationToken ct)
        {
            ApplyInclude();
            if(!string.IsNullOrWhiteSpace(request.Id) && Guid.TryParse(request.Id, out var id))
            {
                Query = Query.Where(x => x.Id == id);
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                Query = Query.Where(x => EF.Functions.Like(EF.Functions.Unaccent(x.Name).ToLower().Trim(),
                                            EF.Functions.Unaccent(request.Name).ToLower().Trim())
                                    );
            }
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
                dbContext.Update(product);
                var toAdd = request.CategoryIds.Except(product.ProductCategories.Select(x => x.CategoryId)).ToList();
                var toRemove = product.ProductCategories.ExceptBy(request.CategoryIds, x => x.CategoryId).Select(x => x.CategoryId).ToList();

                if (toAdd.Count == 0 && toRemove.Count == 0) return;

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
                    await dbContext.SaveChangesAsync(ct);
                }
            }
            else
            {
                throw new CException($"Invalid id: {request.Id}");
            }
        }
        private void ApplyInclude()
        {
            Query = Query.Include(x => x.ProductCategories)
                            .ThenInclude(x => x.Category);
        }
        private IQueryable<Product> QueryRequest(PaginationRequest<ProductFilterModel> request)
        {
            ApplyInclude();
            if (request.Filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(request.Filter.Name))
                {
                    Query = Query.Where(x => EF.Functions.Like(EF.Functions.Unaccent(x.Name).ToLower().Trim(), 
                                                EF.Functions.Unaccent(request.Filter.Name).ToLower().Trim())
                                        );
                }
            }
            return Query;
        }
    }
}
