using Client.Application.Models.Category;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;

namespace Client.Application.Abstractions
{
    public interface ICategoryService
    {
        Task<PaginationResponse<CategoryDto>> GetPagination(PaginationRequest<CategoryFilterModel> request, CancellationToken ct);
        Task<CategoryDto> GetDetail(GetDetailCategoryRequest request, CancellationToken ct);
    }
}
