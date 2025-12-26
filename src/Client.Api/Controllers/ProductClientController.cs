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

        /// <summary>
        /// Get top most viewed products
        /// </summary>
        [HttpGet]
        [Route("most-viewed")]
        public async Task<IActionResult> GetMostViewed([FromQuery] int limit = 6, CancellationToken ct = default)
        {
            var command = new GetMostViewedProductsCommand { Limit = limit };
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }

        /// <summary>
        /// Get hot/trending products (high views in last 30 days)
        /// </summary>
        [HttpGet]
        [Route("hot")]
        public async Task<IActionResult> GetHot([FromQuery] int limit = 6, CancellationToken ct = default)
        {
            var command = new GetHotProductsCommand { Limit = limit };
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }
    }
}
