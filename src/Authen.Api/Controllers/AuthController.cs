using Authen.Application.Commands;
using Authen.Application.Models;
using AutoMapper;
using Common.Api;
using Common.Kernel.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.Kernel.Extensions;
namespace Authen.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request, CancellationToken ct)
        {
            var command = mapper.Map<RegisterCommand>(request);
            await mediator.Send(command, ct);
            return this.CreateOk();
        }
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshDto request, CancellationToken ct)
        {
            var command = mapper.Map<LogoutCommand>(request);
            await mediator.Send(command, ct);
            return this.CreateOk();
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request, CancellationToken ct)
        {
            var command = mapper.Map<LoginCommand>(request);
            var result = await mediator.Send(command, ct);
            return this.CreateOk(result);
        }
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto request, CancellationToken ct)
        {
            var command = mapper.Map<RefreshCommand>(request);
            var result = await mediator.Send(command, ct);
            return this.CreateOk(result);
        }
        [HttpGet]
        [Route("region")]
        public Task<IActionResult> GetRegion(CancellationToken ct)
        {
            var result = typeof(Region).GetValues<string>();
            return Task.FromResult(this.CreateOk(result));
        }
    }
}
