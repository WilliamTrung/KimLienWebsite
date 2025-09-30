using Admin.Application.Abstractions;
using Admin.Application.Models.Category;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.Infrastructure.Pagination;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;

namespace Admin.Infrastructure.Services
{
    public class CategoryService(AdminDbContext dbContext,
        IMapper mapper)
        : PaginationServiceBase<Category, PaginationRequest<CategoryFilterModel>, CategoryDto>(mapper, dbContext)
        , ICategoryService
    {
        public Task<Guid> Create(CreateCategoryDto category, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetDetail(GetDetailCategoryRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<CategoryDto>> GetPagination(PaginationRequest<CategoryFilterModel> request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task Update(ModifyCategoryDto category, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
