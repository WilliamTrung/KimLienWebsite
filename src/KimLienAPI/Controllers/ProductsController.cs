using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Entities;
using AppService.UnitOfWork;
using AppService.Paging;
using System.Net.NetworkInformation;
using AppService.Models;
using AppService.Extension;

namespace KimLienAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery]PagingRequest paging, [FromQuery]List<string> categories, [FromQuery]string name)
        {
            if(paging == null)
            {
                paging = new PagingRequest();
            }
            var products =  await _unitOfWork.ProductService.GetProductModels(paging: paging, filter: p=> Helper.MinimalCompareString(p.Name, name));
            products = _unitOfWork.ProductService.CheckCategories(products, categories);
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(Guid id)
        {
            var product = await _unitOfWork.ProductService.GetProductModels(filter: p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.FirstOrDefault());
        }
    }
}
