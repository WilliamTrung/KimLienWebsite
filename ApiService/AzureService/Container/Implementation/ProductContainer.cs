using ApiService.AzureService.Container.Implementation;
using ApiService.ServiceAdministrator;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Azure.Container.Implementation
{
    public class ProductContainer : BaseContainer, IProductContainer
    {
        public ProductContainer(BlobContainerClient container) : base(container)
        {
        }

        public async Task<List<string>> Upload(Guid productId, List<IFormFile> pictures)
        {
            var result = new List<string>();
            foreach (var picture in pictures)
            {
                var tick = DateTime.UtcNow.Ticks.ToString();
                var name = productId.ToString() + "-" + tick.ToString();
                var uploaded = await UploadAsync(picture, name);
                result.Add(uploaded);
            }
            return result;
        }

        public Task<List<string>> GetProductPictureURLs()
        {
            throw new NotImplementedException();
        }
    }
}
