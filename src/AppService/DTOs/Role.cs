using System;
using System.ComponentModel.DataAnnotations;

namespace AppService.DTOs
{
    public partial class Role
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Role must have a name!")]
        public string? Name { get; set; }
    }
}
