using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceModels.Auth
{
    public interface ITokenModel
    {
        Guid Id { get; set; }
        Models.Enum.Role Role { get; set; }
    }
    public class TokenModel : ITokenModel
    {
        public Guid Id { get; set; }
        public Models.Enum.Role Role { get; set; }
    }
}
