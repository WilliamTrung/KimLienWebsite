using Authen.Application.Abstractions;
using Authen.Application.Commands;
using Authen.Application.Models;
using AutoMapper;
using Common.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return this.CreateOkForResponse(result);
        }
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto request, CancellationToken ct)
        {
            var command = mapper.Map<RefreshCommand>(request);
            var result = await mediator.Send(command, ct);
            return this.CreateOkForResponse(result);
        }
    }
}
