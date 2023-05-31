using System;
using System.ComponentModel.DataAnnotations;

namespace ApiService.DTOs
{
    public partial class User
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Password is required!")]
        public string? Password { get; set; }
        [Required]
        public Guid RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
