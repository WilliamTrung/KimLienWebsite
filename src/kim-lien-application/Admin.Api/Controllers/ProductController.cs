using Admin.Application.Commands.Product;
using Admin.Application.Models.Product;
using Common.Api;
using Common.Kernel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    [Route("/api/management/product")]
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
        [Route("{slug}")]
        public async Task<IActionResult> GetDetail([FromRoute]string slug, CancellationToken ct)
        {
            var command = new GetProductDetailCommand
            {
                Value = slug,
            };
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommand request, CancellationToken ct)
        {
            await sender.Send(request, ct);
            return this.CreateOk();
        }
        [HttpPatch]
        [Route("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute]string id ,[FromBody] UpdateProductStatusRequest request, CancellationToken ct)
        {
            var updateStatusCommand = new UpdateProductStatusCommand
            {
                Id = id,
                Status = request.Status,
            };
            await sender.Send(updateStatusCommand, ct);
            return this.CreateOk();
        }
    }
}
