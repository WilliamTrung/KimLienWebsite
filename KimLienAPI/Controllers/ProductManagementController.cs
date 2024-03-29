using KimLienAPI.Helper;
using KL_Service.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models.ApiParams.Product;
using Models.Enum;
using Models.ServiceModels.Product.Operation;

namespace KimLienAPI.Controllers
{
    [Authorize(Role.Administrator)]
    [Route("api/management/product")]
    [ApiController]
    public class ProductManagementController : ODataController
    {
        private readonly IProductManagementService _productManagementService;
        public ProductManagementController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }
        [HttpGet]
        [EnableQuery]
        public IActionResult GetProducts()
        {
            var products = _productManagementService.GetProductAdminView();
            return Ok(products);
        }
        [HttpGet]
        [Route("{productId}")]
        public IActionResult GetProductById([FromRoute]Guid productId)
        {
            var products = _productManagementService.GetProductAdminById(productId);
            return Ok(products);
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddProduct([FromForm]ProductAddApiModel product)
        {
            var createdId = await _productManagementService.AddProduct(product);
            return StatusCode(StatusCodes.Status201Created, createdId);
        }
        [HttpPatch]
        [Route("modify")]
        public async Task<IActionResult> ModifyProduct([FromBody]ProductModifyModel product)
        {
            await _productManagementService.ModifyProduct(product);
            return NoContent();
        }
        #region category-product-management
        [HttpGet]
        [EnableQuery]
        [Route("main-categories")]
        public IActionResult GetMainCategories(Guid productId)
        {
            var result = _productManagementService.GetMainCategories(productId);
            return Ok(result);
        }
        [HttpGet]
        [EnableQuery]
        [Route("sub-categories")]
        public IActionResult GetSubCategories(Guid productId, Guid parentId)
        {
            var result = _productManagementService.GetSubCategories(productId, parentId);
            return Ok(result);
        }
        [HttpPatch]
        [Route("add-category")]
        public async Task<IActionResult> AddCategoryToProduct([FromBody]ProductCategoryAddModel model)
        {
            await _productManagementService.AddCategoryToProduct(model);
            return NoContent();
        }
        [HttpPatch]
        [Route("remove-category")]
        public async Task<IActionResult> RemoveCategoryFromProducr([FromBody] ProductCategoryModel model)
        {
            await _productManagementService.RemoveCategoryFromProduct(model);
            return NoContent();
        }
        #endregion
        #region image-management
        [HttpPatch]
        [Route("add-image")]
        public async Task<IActionResult> AddProductImage([FromForm]ProductImageAddModel model)
        {
            await _productManagementService.AddImage(model.ProductId, model.Images);
            return NoContent();
        }
        [HttpPatch]
        [Route("adjust-position")]
        public async Task<IActionResult> AdjustImagePosition(ProductImagePositionModel model)
        {
            await _productManagementService.AdjustImagesPosition(model.ProductId, model.Images);
            return NoContent();
        }
      
        [HttpDelete]
        [Route("delete-image/{productId}")]
        public async Task<IActionResult> DeleteImage([FromRoute]Guid productId, [FromBody] string imageUrl)
        {
            await _productManagementService.DeleteImage(productId, imageUrl);
            return NoContent();
        }
        #endregion
    }
}
