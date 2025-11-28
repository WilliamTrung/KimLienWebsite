using Common.Kernel.Constants;
using Import.Application.Commands.Legacy;
using Import.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Import.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    [Route("/api/import")]
    public class ImportController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [Route("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { Status = "Import API is running." });
        }
        [HttpPost("import-data")]
        public async Task<IActionResult> ImportData(CancellationToken ct)
        {
            try
            {
                var importEvent = new ImportDataEvent();
                await sender.Send(importEvent, ct);
                return Ok(new { Message = "Data import initiated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while importing data.", Details = ex.Message });
            }
        }
        [HttpPost("import-product-category")]
        public async Task<IActionResult> ImportProductCategory(CancellationToken ct)
        {
            try
            {
                var importEvent = new ImportLegacyProductCategoryCommand();
                await sender.Send(importEvent, ct);
                return Ok(new { Message = "Data import initiated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while importing data.", Details = ex.Message });
            }
        }
    }
}
