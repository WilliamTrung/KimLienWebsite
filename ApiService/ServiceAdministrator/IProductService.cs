using ApiService.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator
{
    public interface IProductService
    {
        Product? GetById(Guid id);
        IEnumerable<Product> GetProducts();
        /// <summary>
        /// Update product
        /// <para>Throw DuplicateNameException: Duplicated name</para>
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="DuplicateNameException"></exception>
        void Update(Product product);
        //void AdjustPicture(Product product);
        /// <summary>
        /// Disable a product
        /// Throw KeyNotFoundException: No product with specified guid
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        void Disable(Guid id);
        /// <summary>
        /// Add new product
        /// <para>Throw DuplicateNameException: Duplicated name</para>
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="DuplicateNameException"></exception>
        void Add(Product product);
        IEnumerable<Product> GetProducts_ProductCategory(List<string> categories);
        void SetPrimaryPicture(Guid productId, string picture);
        /// <summary>
        /// For uploading product image to azure service named by productId
        /// Update product pictures' record
        /// <para>Throw AggregateException: Upload failed</para>
        /// <para>Throw KeyNotFoundException: No product has such id found</para>
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictures"></param>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        Task Upload(Guid productId, List<IFormFile> pictures);
        /// <summary>
        /// Delete images from product and update db
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictures"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        Task DeleteImage(Guid productId, List<string> pictures);
    }
}
