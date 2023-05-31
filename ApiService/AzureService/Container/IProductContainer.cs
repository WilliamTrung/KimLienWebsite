using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Azure.Container
{
    public interface IProductContainer : IBaseContainer
    {
        /// <summary>
        /// Upload pictures to azure service based on productId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictures"></param>
        /// <returns>List of uploaded pictures' url (empty if failed)</returns>
        /// <exception cref="ArgumentNullException">No product with passed productId param found!</exception>
        Task<List<string>> Upload(Guid productId, List<IFormFile> pictures);
        Task<List<string>> GetProductPictureURLs();
    }
}
