using Admin.Application.Models.Category;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;

namespace Admin.Application.Abstractions
{
    public interface ICategoryService
    {
        Task Update(ModifyCategoryDto category, CancellationToken ct);
        Task<Guid> Create(CreateCategoryDto category, CancellationToken ct);
        Task Delete(Guid id, CancellationToken ct);
        Task<PaginationResponse<CategoryDto>> GetPagination(PaginationRequest<CategoryFilterModel> request, CancellationToken ct);
        Task<CategoryDto> GetDetail(GetDetailCategoryRequest request, CancellationToken ct);
    }
}
