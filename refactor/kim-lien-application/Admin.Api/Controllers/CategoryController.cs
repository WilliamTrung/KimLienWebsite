using Admin.Application.Commands.Category;
using Common.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers
{
    [ApiController]
    [Route("/api/manage/category")]
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
        [Route("detail")]
        public async Task<IActionResult> GetDetail([FromQuery] GetCategoryDetailCommand request, CancellationToken ct)
        {
            var result = await sender.Send(request, ct);
            return this.CreateOk(result);
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand request, CancellationToken ct)
        {
            await sender.Send(request, ct);
            return this.CreateOk();
        }
    }
}
