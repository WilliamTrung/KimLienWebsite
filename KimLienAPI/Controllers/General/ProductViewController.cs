using ApiService.ServiceCustomer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.General
{
    [Route("api/products")]
    [ApiController]
    public class ProductViewController : ODataController
    {
        private readonly IProductService _productService;

        public ProductViewController(IProductService productService)
        {
            _productService= productService;
        }
        // GET: api/<ProductController>
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var result = _productService.GetProducts();
            return Ok(result);  
        }
    }
}
