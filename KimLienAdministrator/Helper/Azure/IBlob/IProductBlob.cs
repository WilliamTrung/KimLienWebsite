using AppService.DTOs;
using Microsoft.AspNetCore.Http;

namespace KimLienAdministrator.Helper.Azure.IBlob
{
    public interface IProductBlob
    {
        public Task<bool> UploadAsync(List<IFormFile> files, Guid productId);
        public List<string>? GetURL(Product product);
        Task<bool> DeleteAsync(Guid productId, string picture);
    }
}
