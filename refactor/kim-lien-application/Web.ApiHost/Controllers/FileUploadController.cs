using Common.Api;
using Common.Application.Storage.Extensions;
using Common.Application.Storage.Helpers;
using Common.Infrastructure.Storage.Azure.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.ApiHost.Controllers
{
    [ApiController]
    [Route("api/file-upload")]
    public class FileUploadController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm]IFormFile fileData, CancellationToken ct)
        {
            var (tags, metadata) = TagHelper.BuildDeletedTags(DateTimeOffset.UtcNow);
            var fileUpload = await fileData.ToFileUploadAsync(
                destinationPrefix: "general"
                , publicRead: true
                , tags: tags
                , metadata: metadata
                , cancellationToken: ct);
            var command = new AzureUploadFileCommand();
            var result = await sender.Send(command, ct);
            return this.CreateOk(result);
        }
    }
}
