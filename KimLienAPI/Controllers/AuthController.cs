using KL_Service.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ServiceModels.Auth;

namespace KimLienAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public IActionResult Login(LoginRequestModel loginRequest)
        {
            loginRequest.Id = Guid.Parse("fbea9802-b624-42ea-82bf-5fe58a88474d");
            loginRequest.Password = "123";
            var token = _authService.GetToken(loginRequest);
            return Ok(token);
        }
    }
}
