using AppService.DTOs;
using AppService.Extension;
using AppService.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Service
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _session;
        public AuthService(IHttpContextAccessor session)
        {
            _session= session;
        }

        public bool Login(User user)
        {
            var result = _session.HttpContext.Session.Login(user);
            if (result)
            {
                return true;
            }
            return false;
        }

        public void Logout()
        {
            _session.HttpContext.Session.Logout();
        }
    }
}
