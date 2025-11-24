using Admin.Application.Abstractions;
using Admin.Application.Commands.User;
using MediatR;

namespace Admin.Infrastructure.Handlers.User
{
    public class ModifyUserHandler(IUserService userService) : IRequestHandler<ModifyUserCommand>
    {
        public async Task Handle(ModifyUserCommand request, CancellationToken cancellationToken)
        {
            await userService.ModifyUser(request, cancellationToken);
        }
    }
}
