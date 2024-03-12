using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.User
{
    public class LoginModel
    {
        public string Id { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
