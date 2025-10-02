using System.ComponentModel.DataAnnotations;

namespace Authen.Application.Models
{
    public class RegisterDto
    {
        [Required, EmailAddress] public string Email { get; set; } = default!;
        [Required, MinLength(8)] public string Password { get; set; } = default!;
        [Required, Phone]
        public string PhoneNumber { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string? DisplayName { get; set; }
    }
}
