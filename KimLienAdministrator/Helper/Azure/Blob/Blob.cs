using Azure.Storage.Blobs;
using KimLienAdministrator.Helper.Azure.IBlob;
using System.ComponentModel;

namespace KimLienAdministrator.Helper.Azure.Blob
{
    public class Blob : IBlobService
    {
        IConfiguration _config;

        private BlobServiceClient? blobStorage;
        private BlobContainerClient? blobContainer;

        public Blob(IConfiguration config)
        {
            _config = config;
            blobStorage = AzureService.GetBlobServiceClient(_config);
            var blobStorage_config = _config.GetSection("BlobStorage");
        }
        public List<string>? GetURLs(string container, int id)
        {
            List<string>? result = null;
            if(blobStorage != null)
                blobContainer = AzureService.CheckBlobContainerAsync(blobStorage, container).Result;
            if (blobContainer == null)
                return result;
            
            string prefix = id + "_";
            var blobs = blobContainer.GetBlobs(prefix: prefix);
            string url_prefix = blobContainer.Uri.ToString();
            result = new List<string>();
            foreach (var blob in blobs)
            {
                string url = url_prefix + "/" + blob.Name;
                result.Add(url);
            }
            //result = blob.Uri.ToString();
            return result;
        }

        public async Task<bool> UploadAsync(List<IFormFile> files, string container, int id)
        {
            try
            {
                if (blobStorage == null)
                    return false;
                blobContainer = AzureService.CheckBlobContainerAsync(blobStorage, container).Result;
                if (blobContainer != null)
                {
                    int i = 0;
                    string prefix = id.ToString() + "_";
                    foreach (var file in files)
                    {
                        string extension = file.ContentType.Split("/")[1];
                        string filename = id + "_" + (++i) + "." + extension;
                        var blob = blobContainer.GetBlobClient(filename);
                        var stream = file.OpenReadStream();
                        await blob.UploadAsync(stream, overwrite: true);

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
