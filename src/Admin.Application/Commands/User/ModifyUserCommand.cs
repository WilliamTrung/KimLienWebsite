using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Admin.Application.Commands.User
{
    public class ModifyUserCommand : IRequest
    {
        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        [MinLength(8)] 
        public string? Password { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Region { get; set; }
        public string? DisplayName { get; set; }
    }
}
