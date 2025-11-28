using System.ComponentModel.DataAnnotations;

namespace Authen.Application.Models
{
    public class RefreshDto
    {
        [Required] public string RefreshToken { get; set; } = default!;
    }
}
