using Admin.Application.Abstractions;
using Admin.Application.Commands.Category;
using Admin.Application.Models.Category;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Admin.Infrastructure.Handlers.Category
{
    public class GetCategoryPaginationCommandHandler(ICategoryService categoryService) : IRequestHandler<GetCategoryPaginationCommand, PaginationResponse<CategoryDto>>
    {
        public async Task<PaginationResponse<CategoryDto>> Handle(GetCategoryPaginationCommand request, CancellationToken cancellationToken)
            => await categoryService.GetPagination(request, cancellationToken);
    }
}
