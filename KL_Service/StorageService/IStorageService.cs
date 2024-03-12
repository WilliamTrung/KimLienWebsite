using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_Service.StorageService
{
    public interface IStorageService
    {
        Task<string> UploadFile(IFormFile file);
        Task DeleteFile(string fileUrl);
    } 
    public class StorageService : IStorageService
    {
        internal readonly Azure.Storage.Blobs.BlobServiceClient _blobClient;
        internal readonly BlobContainerClient _containerClient;
        public StorageService(Azure.Storage.Blobs.BlobServiceClient blobClient, string containerName)
        {

            _blobClient = blobClient;
            _containerClient = _blobClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task DeleteFile(string fileUrl)
        {
            await _containerClient.GetBlobClient(fileUrl).DeleteIfExistsAsync();
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var generated = Guid.NewGuid();
            var fileName = generated+extension;
            var blob = _containerClient.GetBlobClient(fileName);
            await blob.UploadAsync(file.OpenReadStream(), overwrite: true);
            return blob.Uri.AbsoluteUri;
        }
    }
}
