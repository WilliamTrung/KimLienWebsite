using AutoMapper;
using Client.Application.Abstractions;
using Client.Application.Models.Product;
using Client.Infrastructure.Data;
using Common.Domain.Entities;
using Common.Infrastructure;
using Common.Infrastructure.Pagination;
using Common.Kernel.Dependencies;
using Common.Kernel.Extensions;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Client.Infrastructure.Services
{
    public class ProductService : PaginationServiceBase<Product, PaginationRequest<ProductFilterModel>, ProductDto>
        , IProductService, IScoped
    {
        public ProductService(IMapper mapper, ClientDbContext dbContext) : base(mapper, dbContext)
        {
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
        private void ApplyInclude()
        {
            Query = Query.Include(x => x.ProductCategories)
                            .ThenInclude(x => x.Category)
                            .ThenInclude(x => x.Parent);
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
