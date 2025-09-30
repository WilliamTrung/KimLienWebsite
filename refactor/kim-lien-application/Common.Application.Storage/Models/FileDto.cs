using Microsoft.AspNetCore.Http;

namespace Common.Application.Storage.Models
{
    public sealed class FileDto
    {
        public IFormFile Data { get; set; } = null!;
        public string? CustomName { get; set; }
    }
}
