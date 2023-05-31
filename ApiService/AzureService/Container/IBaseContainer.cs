using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Azure.Container
{
    public interface IBaseContainer : IDisposable
    {
        public Task<string> UploadAsync(IFormFile file, string filename);
        public Task DeleteAsync(string filename);
        public List<string>? GetURLs(string prefix_name);
    }
}
