using ApiService.Azure.Container;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiService.AzureService.Container.Implementation
{
    public class BaseContainer : IBaseContainer
    {
        private bool disposedValue;
        internal BlobContainerClient _container;
        public BaseContainer(BlobContainerClient container)
        {
            _container= container;
        }

        public async Task DeleteAsync(string filename)
        {
            var split = Regex.Split(filename, @"[/\\]");
            filename = split.Last();
            var blob = _container.GetBlobClient(filename);
            await blob.DeleteIfExistsAsync();
        }


        public List<string>? GetURLs(string prefix_name)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(IFormFile file, string filename)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);
            string fullname = filename + extension;
            var blob = _container.GetBlobClient(fullname);
            var item = await blob.UploadAsync(file.OpenReadStream(), overwrite: true);
            Console.WriteLine(item.Value);
            return blob.Uri.AbsoluteUri;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseContainer()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
