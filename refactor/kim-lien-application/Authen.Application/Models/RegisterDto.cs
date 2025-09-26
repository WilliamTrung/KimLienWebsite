using System.ComponentModel.DataAnnotations;

namespace Authen.Application.Models
{
    public class RegisterDto
    {
        [Required, EmailAddress] public string Email { get; set; } = default!;
        [Required, MinLength(8)] public string Password { get; set; } = default!;
        public string? DisplayName { get; set; }
    }
}
