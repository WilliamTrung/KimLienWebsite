using AutoMapper;
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
    public class FileUploadController(ISender sender, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileData file)
        {
            var (tags, metadata) = TagHelper.BuildDeletedTags(DateTimeOffset.UtcNow);
            var fileUpload = await file.File.ToFileUploadAsync(
                destinationPrefix: "general"
                , publicRead: true
                , tags: tags
                , metadata: metadata
                , autoGenerateFileName: true);
            var command = mapper.Map<AzureUploadFileCommand>(fileUpload);
            command.ProfileKey = "Picture";
            var result = await sender.Send(command);
            return this.CreateOk(result);
        }
    }
    public class FileData
    {
        public IFormFile File { get; set; } = null!;
    }
}
