using Admin.Application.Commands.User;
using Admin.Application.Models.User;
using Common.Kernel.Response.Pagination;

namespace Admin.Application.Abstractions
{
    public interface IUserService
    {
        Task CreateUser(CreateUserCommand request, CancellationToken ct);
        Task ModifyUser(ModifyUserCommand request, CancellationToken ct);
        Task Delete(Guid id, CancellationToken ct);
        Task<PaginationResponse<UserDto>> GetPaginationResponse(QueryUserCommand request, CancellationToken ct);
        Task<UserDto> GetDetail(GetDetailUserCommand request, CancellationToken ct);
    }
}
