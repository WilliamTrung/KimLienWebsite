using AppService.DTOs;
using AppService.UnitOfWork;
using Azure.Storage.Blobs;
using KimLienAdministrator.Helper.Azure.IBlob;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace KimLienAdministrator.Helper.Azure.Blob
{
    public class ProductBlob : IProductBlob
    {
        IConfiguration _config;
        IUnitOfWork _unitOfWork;

        BlobServiceClient? blobStorage;
        BlobContainerClient? blobContainer;

        private string container;
        public ProductBlob(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            blobStorage = AzureService.GetBlobServiceClient(_config);

            var blobStorage_config = _config.GetSection("BlobStorage");
            container = blobStorage_config["ProductContainer"];

            blobContainer = AzureService.CheckBlobContainerAsync(blobStorage, container).Result;//blobStorage.GetBlobContainerClient(container);
        }

        public List<string>? GetURL(Product product)
        {
            //throw new NotImplementedException();
            List<string>? result = null;
            if (blobStorage == null || blobContainer == null)
                return result;
            //string blobName = product.Id.ToString() + ".png";
            string prefix = product.Id.ToString() + "_";
            var blobs = blobContainer.GetBlobs(prefix: prefix);
            string url_prefix = blobContainer.Uri.ToString();
            result = new List<string>();
            foreach(var blob in blobs)
            {
                string url = url_prefix +"/"+ blob.Name;
                result.Add(url);
            }
            //result = blob.Uri.ToString();
            return result;
        }
        public async Task<bool> UploadAsync(List<IFormFile> files, Guid productId)
        {
            //throw new NotImplementedException();
            try
            {
                if (blobStorage == null)
                    return false;
                var product = (await _unitOfWork.ProductService.GetDTOs(filter: p => p.Id == productId)).FirstOrDefault();
                if (product == null)
                    return false;
                if (blobContainer != null)
                {
                    int i = 0;
                    string prefix = product.Id.ToString() + "_";
                    foreach (var file in files)
                    {
                        string extension = file.ContentType.Split("/")[1];
                        string filename = productId.ToString() + "_"+ (++i) + "." + extension;
                        var blob = blobContainer.GetBlobClient(filename);
                        var stream = file.OpenReadStream();
                        var item = await blob.UploadAsync(stream, overwrite: true); 
                    }
                    var pictures = GetURL(product);
                    string combine = "";
                    if(pictures != null)
                    {
                        foreach (var picture in pictures)
                        {
                            combine += picture + ",";
                        }
                    }
                    product.Pictures = combine;
                    var result = await _unitOfWork.ProductService.Update(p => p.Id == product.Id, product);
                    return true;
                }
                else
                {
                    return false;
                }
            } catch
            {
                return false;
            }

        }

    }
}
