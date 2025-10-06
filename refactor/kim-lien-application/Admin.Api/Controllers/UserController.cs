using Admin.Application.Commands.User;
using Common.Api;
using Common.Kernel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    [Route("/api/management/user")]
    public class UserController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand request, CancellationToken ct)
        {
            await sender.Send(request, ct);
            return this.CreateOk();
        }
        //[HttpPut]
        //[Route("modify")]
        //public async Task<IActionResult> Modify([FromBody] ModifyUserCommand request, CancellationToken ct)
        //{
        //    await sender.Send(request, ct);
        //    return this.CreateOk();
        //}
        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] QueryUserCommand request, CancellationToken ct)
        {
            var result = await sender.Send(request, ct);
            return this.CreateOk(result);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> GetDetail([FromRoute] string slug, CancellationToken ct)
        {
            var command = new GetDetailUserCommand
            {
                Value = slug,
            };
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }
    }
}
