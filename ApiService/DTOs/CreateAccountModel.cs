using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.DTOs
{
    public class CreateAccountModel
    {
        [MinLength(1, ErrorMessage = "Password is required!")]
        public string? Password { get; set; }
        [Required]
        public Guid RoleId { get; set; }
    }
}
