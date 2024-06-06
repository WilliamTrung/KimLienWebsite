using KimLienAPI.Helper;
using KL_Service.CustomerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models.ApiParams.CustomerQuery;
using Models.Enum;

namespace KimLienAPI.Controllers
{
    //[Authorize(Role.General)]
    [Route("api/customer")]
    [ApiController]
    public class ProductCustomerController : ODataController
    {
        private readonly IProductCustomerService _productCustomerService;
        public ProductCustomerController(IProductCustomerService productCustomerService)
        {
            _productCustomerService = productCustomerService;
        }
        [HttpGet]
        [EnableQuery]
        [Route("product")]
        public IActionResult GetProductCustomerView([FromQuery]ProductQuery? query)
        {
            var products = _productCustomerService.GetProducts(name: query?.Name, categories: query?.Categories);
            return Ok(products);
        }
        [HttpGet]
        [Route("product/{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _productCustomerService.GetProductAsync(id);
            return Ok(product);
        }
        [HttpGet]
        [EnableQuery]
        [Route("categories")]
        public IActionResult GetCategories()
        {
            var categories = _productCustomerService.GetCategories();
            return Ok(categories);
        }
    }
}
