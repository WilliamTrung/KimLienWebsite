using ApiService.AzureService.Container.Implementation;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Azure.Container.Implementation
{
    public class PictureContainer : BaseContainer, IPictureContainer
    {

        public PictureContainer(BlobContainerClient container) : base(container) 
        {
            _container = container;
        }
    }
}
