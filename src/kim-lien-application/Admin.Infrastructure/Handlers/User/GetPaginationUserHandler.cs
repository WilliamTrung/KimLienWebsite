using Admin.Application.Abstractions;
using Admin.Application.Commands.User;
using Admin.Application.Models.User;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Admin.Infrastructure.Handlers.User
{
    public class GetPaginationUserHandler(IUserService userService) : IRequestHandler<QueryUserCommand, PaginationResponse<UserDto>>
    {
        public async Task<PaginationResponse<UserDto>> Handle(QueryUserCommand request, CancellationToken cancellationToken)
        {
            return await userService.GetPaginationResponse(request, cancellationToken);
        }
    }
}
