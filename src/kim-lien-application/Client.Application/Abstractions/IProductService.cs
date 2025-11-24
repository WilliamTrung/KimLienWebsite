using Client.Application.Models.Product;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;

namespace Client.Application.Abstractions
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetPaginationResponse(PaginationRequest<ProductFilterModel> request, CancellationToken ct);
        Task<ProductDto> GetDetail(GetDetailProductRequest request, CancellationToken ct);
    }
}
