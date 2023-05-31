using ApiService.Azure.Container;
using ApiService.Azure.Container.Implementation;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Azure
{
    public class AzureService : IAzureService
    {
        private BlobServiceClient _storage;
        private IConfiguration _config;

        private IProductContainer _productContainer = null!;
        private IPictureContainer _pictureContainer = null!;

        public IProductContainer ProductContainer
        {
            get
            {
                if(_productContainer == null)
                {
                    _productContainer = new ProductContainer(GetContainer(_config.GetRequiredSection("BlobStorage")[Global.Azure.ProductContainer_Test]));
                }
                return _productContainer;
            }
        }

        public IPictureContainer PictureContainer
        {
            get
            {
                if (_pictureContainer == null)
                {
                    _pictureContainer = new PictureContainer(GetContainer(_config.GetRequiredSection("BlobStorage")[Global.Azure.PictureContainer_Test]));
                }
                return _pictureContainer;
            }
        }


        public AzureService(IConfiguration config)
        {
            _config = config;
            var blobStorage = _config.GetSection("BlobStorage");
            var connection = blobStorage["AzureWebJobsStorage"];
            _storage = new BlobServiceClient(connection);                  
        }

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
        private BlobContainerClient GetContainer(string containerName)
        {
            BlobContainerClient blobContainer;
            try
            {
                blobContainer = _storage.CreateBlobContainer(containerName, PublicAccessType.Blob);
            }
            catch
            {
                blobContainer = _storage.GetBlobContainerClient(containerName);
            }
            return blobContainer;
        }
    }
}
