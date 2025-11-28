using Admin.Application.Abstractions;
using Admin.Application.Commands.User;
using Admin.Application.Models.User;
using MediatR;

namespace Admin.Application.Handlers.User
{
    public class GetDetailUserHandler(IUserService userService) : IRequestHandler<GetDetailUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(GetDetailUserCommand request, CancellationToken cancellationToken)
        {
             return await userService.GetDetail(request, cancellationToken);
        }
    }
}
