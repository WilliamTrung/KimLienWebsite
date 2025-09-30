using Common.Application.Storage.Models;

namespace Common.Application.Storage.Abstraction
{
    public interface ICloudStorageService
    {
        Task UploadFile(FileDto file);
        Task UploadFile(List<FileDto> files);
        Task DeleteFile(string fileUrl);
        Task MoveFile(string fileUrl, string destinationUrl);
    }
}
