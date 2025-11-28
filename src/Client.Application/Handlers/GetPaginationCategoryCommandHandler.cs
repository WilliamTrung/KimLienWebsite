using Client.Application.Abstractions;

namespace Client.Application.Handlers
{
    public class GetPaginationCategoryCommandHandler(ICategoryService categoryService)
        : MediatR.IRequestHandler<Client.Application.Commands.Category.GetPaginationCategoryCommand, Common.Kernel.Response.Pagination.PaginationResponse<Client.Application.Models.Category.CategoryDto>>
    {
        public async Task<Common.Kernel.Response.Pagination.PaginationResponse<Client.Application.Models.Category.CategoryDto>> Handle(Client.Application.Commands.Category.GetPaginationCategoryCommand request, CancellationToken cancellationToken)
        {
            return await categoryService.GetPagination(request, cancellationToken);
        }
    }
}
