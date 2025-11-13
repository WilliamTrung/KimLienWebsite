using Client.Application.Models.Category;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Client.Application.Commands.Category
{
    public class GetPaginationCategoryCommand : PaginationRequest<CategoryFilterModel>, IRequest<PaginationResponse<CategoryDto>>
    {
    }
}
