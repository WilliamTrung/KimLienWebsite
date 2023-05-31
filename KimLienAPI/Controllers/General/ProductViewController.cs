using ApiService.ServiceCustomer;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.General
{
    [Route("api/products")]
    [ApiController]
    public class ProductViewController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductViewController(IProductService productService)
        {
            _productService= productService;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _productService.GetProducts();
            return Ok(result);  
        }
    }
}
