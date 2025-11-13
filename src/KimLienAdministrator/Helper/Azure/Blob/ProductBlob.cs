using AppService.DTOs;
using AppService.UnitOfWork;
using Azure.Storage.Blobs;
using KimLienAdministrator.Helper.Azure.IBlob;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NuGet.Packaging.Signing;
using System.Collections.Generic;

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
            //blobStorage = services.GetRequiredService(typeof(BlobServiceClient));
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
        private string GetNameFromURI(string uri)
        {
            return uri.Split("/")[4];
        }
        public async Task<bool> DeleteAsync(Guid productId, string picture)
        {
            //https://kimlien1808.blob.core.windows.net/products/fa53df43-bf29-4dbc-5495-08dad14a485b_1.png
            try
            {
                if (blobStorage == null)
                    return false;
                //check product
                var product = (await _unitOfWork.ProductService.GetDTOs(filter: p => p.Id == productId)).FirstOrDefault();
                if (product == null)
                    return false;
                if(blobContainer != null)
                {
                    var name = GetNameFromURI(picture);
                    var blob = blobContainer.GetBlobClient(name);
                    var result = await blob.DeleteAsync();

                    var deserializedPics = _unitOfWork.ProductService.GetDeserializedPictures(product).ToList();
                    deserializedPics.Remove(picture);
                    product.Pictures = AppService.Extension.Helper.MergeListString(deserializedPics);
                    var updated = await _unitOfWork.ProductService.Update(p=>p.Id == product.Id, product);
                    return result.Status == 202;
                }
                return false;
            } catch
            {
                return false;
            }
        }
        private int GetPicIdByURI(string uri)
        {
            int result = -1;
            var name = GetNameFromURI(uri);
            var pic_id = name.Split("_")[1].Split(".")[0];
            result = Int32.Parse(pic_id);
            return result;
        }
        public async Task<bool> UploadAsync(List<IFormFile> files, Guid productId)
        {
            //throw new NotImplementedException();
            try
            {
                if (blobStorage == null)
                    return false;
                //check product
                var product = (await _unitOfWork.ProductService.GetDTOs(filter: p => p.Id == productId)).FirstOrDefault();
                if (product == null)
                    return false;
                if (blobContainer != null)
                {
                    var pictures = GetURL(product);
                    int i = 0;
                    if (pictures != null)
                    {
                        pictures = pictures.OrderBy(e => e.Length).ToList();
                        var last = pictures.LastOrDefault();
                        if (last != null)
                        {
                            i = GetPicIdByURI(last);
                        }
                    }
                    string prefix = product.Id.ToString() + "_";
                    foreach (var file in files)
                    {
                        string extension = file.ContentType.Split("/")[1];
                        string filename = productId.ToString() + "_"+ (++i) + "." + extension;
                        var blob = blobContainer.GetBlobClient(filename);
                        var stream = file.OpenReadStream();
                        var item = await blob.UploadAsync(stream, overwrite: true); 
                        
                    }
                    pictures = GetURL(product);
                    string combine = "";
                    if(pictures != null)
                    {
                        pictures = pictures.OrderBy(e => e.Length).ToList();
                        combine = AppService.Extension.Helper.MergeListString(pictures);
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
