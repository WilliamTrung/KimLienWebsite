using Admin.Application.Models.User;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Admin.Application.Commands.User
{
    public class QueryUserCommand : PaginationRequest<UserFilterModel>, IRequest<PaginationResponse<UserDto>>
    {
    }
}
