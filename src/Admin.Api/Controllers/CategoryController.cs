using Admin.Application.Commands.Category;
using Common.Api;
using Common.Kernel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    [Route("/api/management/category")]
    public class CategoryController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand request, CancellationToken ct)
        {
            var result = await sender.Send(request, ct);
            return this.CreateOk(result);
        }
        [HttpPut]
        [Route("modify")]
        public async Task<IActionResult> Modify([FromBody] ModifyCategoryCommand request, CancellationToken ct)
        {
            await sender.Send(request, ct);
            return this.CreateOk();
        }
        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] GetCategoryPaginationCommand request, CancellationToken ct)
        {
            var result = await sender.Send(request, ct);
            return this.CreateOk(result);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> GetDetail([FromRoute] string slug, CancellationToken ct)
        {
            var command = new GetCategoryDetailCommand
            {
                Value = slug,
            };
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteCategoryCommand request, CancellationToken ct)
        {
            await sender.Send(request, ct);
            return this.CreateOk();
        }
    }
}
