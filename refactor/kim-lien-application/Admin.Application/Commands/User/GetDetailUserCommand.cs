using Admin.Application.Models.User;
using MediatR;

namespace Admin.Application.Commands.User
{
    public class GetDetailUserCommand : IRequest<UserDto>
    {
        public string Value { get; set; } = null!; //Guid value or email
    }
}
