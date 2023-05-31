using ApiService.Azure;
using ApiService.ServiceAdministrator;
using KimLienAPI.Model;
using KimLienAPI.Model.Product;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.Administrator
{
    [Route("api/administrator/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IAzureService _azureService;

        public UploadController(IAzureService azureService)
        {
            _azureService = azureService;   
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync(string filename, ImageModel image)
        {
            try
            {
                await _azureService.PictureContainer.UploadAsync(image.Image, filename);
            }
            catch (AggregateException)
            {
                return Ok(StatusCode(StatusCodes.Status503ServiceUnavailable, "Cannot upload image to azure storage"));
            }
            return Ok(StatusCode(StatusCodes.Status202Accepted, "Upload successfully"));
        }
    }
}
