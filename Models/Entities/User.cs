using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class User : IEntityIdentifier
    {
        public Guid Id { get; }
        public string Password { get; set; } = null!;
        public Role Role { get; set; }
    }
}
