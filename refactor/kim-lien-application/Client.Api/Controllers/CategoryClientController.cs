using Client.Application.Commands.Category;
using Common.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Client.Api.Controllers
{
    [ApiController]
    [Route("/api/category")]
    public class CategoryClientController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] GetPaginationCategoryCommand request, CancellationToken ct)
        {
            var result = await sender.Send(request, ct);
            return this.CreateOk(result);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> GetDetail([FromRoute] string slug, CancellationToken ct)
        {
            var command = new GetDetailCategoryCommand
            {
                Value = slug,
            };
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }
    }
}
