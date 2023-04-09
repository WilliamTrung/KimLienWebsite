using AppService.UnitOfWork;
using Azure.Storage.Blobs;
using KimLienAdministrator.Helper.Azure.IBlob;

namespace KimLienAdministrator.Helper.Azure.Blob
{
    public class PictureBlob : IBlobService
    {
        IConfiguration _config;

        BlobServiceClient? blobStorage;
        BlobContainerClient? blobContainer;

        private string container;
        public PictureBlob(IConfiguration config)
        {
            _config = config;
            blobStorage = AzureService.GetBlobServiceClient(_config);
            //blobStorage = services.GetRequiredService(typeof(BlobServiceClient));
            var blobStorage_config = _config.GetSection("BlobStorage");
            container = blobStorage_config["PictureContainer"];

            blobContainer = AzureService.CheckBlobContainerAsync(blobStorage, container).Result;//blobStorage.GetBlobContainerClient(container);
        }
        public List<string>? GetURLs(string container, string name)
        {
            //throw new NotImplementedException();
            List<string>? result = null;
            if (blobStorage == null || blobContainer == null)
                return result;
            var blobs = blobContainer.GetBlobs(prefix: name);
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

        public async Task<bool> UploadAsync(IFormFile file, string filename, string extension)
        {
            try
            {
                if (blobStorage == null)
                    return false;
                if (blobContainer != null)
                {
                    string blobName = filename + "." + extension;
                    var blob = blobContainer.GetBlobClient(blobName);
                    var stream = file.OpenReadStream();
                    var item = await blob.UploadAsync(stream, overwrite: true);
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
