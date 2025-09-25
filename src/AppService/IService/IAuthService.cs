using AppService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.IService
{
    public interface IAuthService
    {
        bool Login(User user);
        void Logout();
    }
}
