using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.User
{
    public class TokenModel
    {
        public string Id { get; set; } = null!;
        public Role Role { get; set; }
    }
}
