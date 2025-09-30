using Admin.Application.Models.Product;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;

namespace Admin.Application.Abstractions
{
    public interface IProductService
    {
        Task<Guid> CreateProduct(CreateProductDto request, CancellationToken ct);
        Task ModifyProduct(ModifyProductDto request, CancellationToken ct);
        Task DeleteProduct(Guid id, CancellationToken ct);
        Task<PaginationResponse<ProductDto>> GetPaginationResponse(PaginationRequest<ProductFilterModel> request, CancellationToken ct);
        Task<ProductDto> GetDetail(GetDetailProductRequest request, CancellationToken ct);
    }
}
