using ApiService.Azure;
using ApiService.DTOs;
using ApiService.UnitOfWork;
using AppCore.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiService.ServiceAdministrator.Implementation
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService) : base(unitOfWork, mapper, azureService)
        {
        }
        /// <summary>
        /// Add new product
        /// <para>Throw DuplicateNameException: Duplicated name</para>
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="DuplicateNameException"></exception>
        public void Add(DTOs.Product product)
        {
            using (var productRepos = _unitOfWork.ProductRepository)
            {
                var products = productRepos.Get();
                var checkNameDuplicated = products.Where(p => Extension.StringExtension.MinimalCompareString(p.Name, product.Name)).FirstOrDefault();
                if (checkNameDuplicated == null)
                {
                    productRepos.Create(_mapper.Map<AppCore.Entities.Product>(product));
                    _unitOfWork.Save();
                }
                else
                {
                    throw new DuplicateNameException(product.Name + " has existed!");
                }
            }
        }

        /// <summary>
        /// Delete images from product and update db
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pictures"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task DeleteImage(Guid productId, List<string> pictures)
        {
            using (var productContainer = _azureService.ProductContainer)
            {
                //update images url to db
                using (var productRepos = _unitOfWork.ProductRepository)
                {
                    var entity = productRepos.GetById(productId);
                    if (entity != null)
                    {
                        List<string> deserialized = entity.Pictures == null ? new List<string>() : Extension.StringExtension.SplitListString(toSplit: entity.Pictures, ",");
                        //check if pictures are valid
                        if (!deserialized.SequenceEqual(pictures))
                        {
                            throw new FileNotFoundException();
                        }
                        //delete image
                        foreach (var picture in pictures)
                        {                            
                            await productContainer.DeleteAsync(picture);
                            deserialized.Remove(picture);
                        }
                        entity.Pictures = Extension.StringExtension.MergeListString(deserialized, ",");
                        _unitOfWork.Save();
                    }
                    else
                    {
                        throw new KeyNotFoundException();
                    }

                }
            }
        }

        /// <summary>
        /// Disable a product
        /// Throw KeyNotFoundException: No product with specified guid
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Disable(Guid id)
        {
            using (var productRepos = _unitOfWork.ProductRepository)
            {
                var product = productRepos.GetById(id);
                if (product != null)
                {
                    productRepos.Delete(product);
                    _unitOfWork.Save();
                }
                else
                {
                    throw new KeyNotFoundException();
                }

            }
        }

        public DTOs.Product? GetById(Guid id)
        {
            using(var productRepos = _unitOfWork.ProductRepository)
            {
                return _mapper.Map<DTOs.Product>(productRepos.GetById(id));
            }
        }

        public IEnumerable<DTOs.Product> GetProducts()
        {
            using (var productRepos = _unitOfWork.ProductRepository)
            {
                return _mapper.Map<IEnumerable<AppCore.Entities.Product>, IEnumerable<DTOs.Product>>(productRepos.Get());
            }
        }

        public IEnumerable<DTOs.Product> GetProducts_ProductCategory(List<string> categories)
        {
            using (var productRepos = _unitOfWork.ProductRepository)
            {
                return _mapper.Map<IEnumerable<AppCore.Entities.Product>, IEnumerable<DTOs.Product>>(productRepos.GetByProductCategory(categories: categories));
            }
        }

        public void SetPrimaryPicture(Guid productId, string picture)
        {
            using (var productRepos = _unitOfWork.ProductRepository)
            {
                var entity = productRepos.GetById(productId);
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                else
                {
                    if (entity.Pictures != null)
                    {
                        entity.DeserializedPictures = Extension.StringExtension.SplitListString(entity.Pictures, ",");
                        if (entity.DeserializedPictures.FirstOrDefault(p => p == picture) != null)
                        {
                            var deserialized = entity.DeserializedPictures.ToList();
                            deserialized.Remove(picture);
                            deserialized.Insert(0, picture);
                            entity.Pictures = Extension.StringExtension.MergeListString(deserialized, ",");

                            productRepos.Update(entity);
                            _unitOfWork.Save();
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(entity.Pictures));
                    }

                }
            }
        }
        /// <summary>
        /// Update product
        /// <para>Throw DuplicateNameException: Duplicated name</para>
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="DuplicateNameException"></exception>
        /// <exception cref="EntryPointNotFoundException"></exception>
        public void Update(DTOs.Product product)
        {
            using (var productRepos = _unitOfWork.ProductRepository)
            {
                //var products = 
                var checkNameDuplicated = productRepos.Get(filter: p => p.Id != product.Id).Where(p => Extension.StringExtension.MinimalCompareString(p.Name, product.Name)).FirstOrDefault();
                if (checkNameDuplicated == null)
                {
                    //var entity = productRepos.GetById(product.Id);
                    //if (entity != null)
                    //{
                    //    entity.Name = product.Name;
                    //    entity.Description = product.Description;
                    //    //entity.
                    //    productRepos.Update(entity);
                    //}
                    //else 
                    //    throw new EntryPointNotFoundException();

                    productRepos.Update(filter: p => p.Id == product.Id, _mapper.Map<AppCore.Entities.Product>(product));
                    _unitOfWork.Save();
                }
                else
                {
                    throw new DuplicateNameException(product.Name + " has existed!");
                }
            }
        }
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
        public async Task Upload(Guid productId, List<IFormFile> pictures)
        {
            using (var productContainer = _azureService.ProductContainer)
            {
                //update images url to db
                using (var productRepos = _unitOfWork.ProductRepository)
                {
                    var entity = productRepos.GetById(productId);
                    if (entity != null)
                    {
                        //check if upload image successfully
                        //upload images
                        var uploaded_pictures = await productContainer.Upload(productId, pictures);
                        if (!uploaded_pictures.Any())
                        {
                            throw new AggregateException("Error at Upload - ProductService - ServiceAdministrator");
                        }
                        List<string> deserialized = entity.Pictures == null ? new List<string>() : Extension.StringExtension.SplitListString(toSplit: entity.Pictures, ",");
                            deserialized.AddRange(uploaded_pictures);
                            entity.Pictures = Extension.StringExtension.MergeListString(deserialized, ",");
                            _unitOfWork.Save();
                        }
                        else
                        {
                            throw new KeyNotFoundException();
                        }

                    }
                }
            }
        }
}
