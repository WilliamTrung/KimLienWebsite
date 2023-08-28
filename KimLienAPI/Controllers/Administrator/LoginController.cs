using ApiService.ServiceAdministrator;
using JwtService;
using KimLienAPI.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.Administrator
{
    [Route("api/administrator/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET: api/<LoginController>
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        public LoginController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }
        // GET api/<LoginController>/5
        [HttpPost]
        public IActionResult Login([FromForm]LoginModel login)
        {
            string pwd = login.Password;
            Extension.HashExtension.Hash(ref pwd);
            var user = _userService.Login(login.Id, pwd);
            if(user == null)
            {
                return Ok(StatusCode(StatusCodes.Status401Unauthorized));
            }
            var access_token = _jwtService.GenerateAccessTokenAsync(user);
            return Ok(access_token);
        }
    }
}
