using Admin.Application.Commands.Category;
using Admin.Application.Commands.Product;
using Common.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers
{
    [ApiController]
    [Route("/api/manage/product")]
    public class ProductController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand request, CancellationToken ct)
        {
            var result = await sender.Send(request, ct);
            return this.CreateOk(result);
        }
        [HttpPut]
        [Route("modify")]
        public async Task<IActionResult> Modify([FromBody] ModifyProductCommand request, CancellationToken ct)
        {
            await sender.Send(request, ct);
            return this.CreateOk();
        }
        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] GetProductPaginationCommand request, CancellationToken ct)
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
        public async Task<IActionResult> Delete([FromBody] DeleteProductCommand request, CancellationToken ct)
        {
            await sender.Send(request, ct);
            return this.CreateOk();
        }
    }
}
