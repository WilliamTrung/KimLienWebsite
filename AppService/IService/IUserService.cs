using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.IService
{
    public interface IUserService : IBaseService<User, DTOs.User>
    {
        public Task<DTOs.User?> Login(DTOs.User user);
        public Task<DTOs.User?> Register(DTOs.User user);
    }
}
