using Azure.Storage.Blobs;
using Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KL_Service.StorageService
{
    public interface IProductContainer : IStorageService
    {
    }
    public class ProductContainer : StorageService, IProductContainer
    {        
        public ProductContainer(BlobServiceClient blobClient) : base(blobClient, StorageContainers.Product)
        {
        }
    }
}
