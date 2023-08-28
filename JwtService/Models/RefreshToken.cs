using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtService.Models
{
    public class RefreshToken
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime CreationDate = DateTime.UtcNow;
        public DateTime ExpirationDate;
    }
}
