using Admin.Application.Abstractions;
using Admin.Application.Commands.User;
using MediatR;

namespace Admin.Infrastructure.Handlers.User
{
    public class CreateUserHandler(IUserService userService) : IRequestHandler<CreateUserCommand>
    {
        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await userService.CreateUser(request, cancellationToken);
        }
    }
}
