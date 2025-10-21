using Client.Application.Commands.Product;
using Common.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Api.Controllers
{
    [ApiController]
    [Route("/api/product")]
    public class ProductClientController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] GetPaginationProductCommand request, CancellationToken ct)
        {
            var result = await sender.Send(request, ct);
            return this.CreateOk(result);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> GetDetail([FromRoute] string slug, CancellationToken ct)
        {
            var command = new GetDetailProductCommand
            {
                Value = slug,
            };
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }
        [HttpPost]
        [Route("favorite-toggle")]
        [Authorize]
        public async Task<IActionResult> FavoriteToggle([FromBody] FavoriteProductCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return this.CreateOk();
        }
    }
}
