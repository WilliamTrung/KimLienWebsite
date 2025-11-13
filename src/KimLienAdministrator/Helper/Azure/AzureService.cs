
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KimLienAdministrator.Helper.Azure
{
    public class AzureService
    {
        public static BlobServiceClient GetBlobServiceClient(IConfiguration _config)
        {
            try
            {
                var blobStorage = _config.GetSection("BlobStorage");
                var connection = blobStorage["AzureWebJobsStorage"];
                BlobServiceClient blob = new BlobServiceClient(connection);
                return blob;
            }
            catch
            {
                throw new Exception("Cannot retrieve blob storage data!");
            }
        }
        public static async Task<BlobContainerClient?> CheckBlobContainerAsync(BlobServiceClient blobStorage, string containerName)
        {
            BlobContainerClient blobContainer;
            try
            {
                blobContainer = (await blobStorage.CreateBlobContainerAsync(containerName, PublicAccessType.Blob)).Value;
            } catch
            {
                blobContainer = blobStorage.GetBlobContainerClient(containerName);
            }
            return blobContainer;
        }
        public static bool CheckBlobClient(BlobContainerClient blobContainer, string client)
        {
            var result = blobContainer.GetBlobClient(client);
            return result == null ? false : true;
        }
        public static string GetBlobResourcePath(IConfiguration _config, string containerName, string fileName)
        {
            string path = _config.GetSection("Images")["Storage"];
            path = Path.Combine(path, containerName, fileName);
            return path;
        }
    }
}
