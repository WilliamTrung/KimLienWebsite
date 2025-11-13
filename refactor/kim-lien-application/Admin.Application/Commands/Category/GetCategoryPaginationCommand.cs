using Admin.Application.Models.Category;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Admin.Application.Commands.Category
{
    public class GetCategoryPaginationCommand : PaginationRequest<CategoryFilterModel>, IRequest<PaginationResponse<CategoryDto>>
    {
    }
}
