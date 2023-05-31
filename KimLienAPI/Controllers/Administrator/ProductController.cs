using ApiService.DTOs;
using ApiService.ServiceAdministrator;
using KimLienAPI.Model;
using KimLienAPI.Model.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.Administrator
{
    [Route("api/administrator/products")]
    [ApiController]
    public class ProductController : ODataController 
    {                        
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService= productService;
        }
        // GET: api/<ProductController>
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var result = _productService.GetProducts().AsQueryable();//.AsQueryable();
            return Ok(result);
        }
        [HttpPost("upload/{id}")]
        public async Task<IActionResult> PostAsync([FromForm]ProductUploadModel productUpload)
        {
            try
            {
                await _productService.Upload(productId: productUpload.Id, productUpload.Files);
            }
            catch (Exception ex)
            {
                if(ex is AggregateException)
                {
                    return Ok(StatusCode(StatusCodes.Status503ServiceUnavailable, "Cannot upload image to azure storage"));
                }
                else if(ex is KeyNotFoundException)
                {
                    return Ok(StatusCode(StatusCodes.Status404NotFound, "No product with Id: " + productUpload.Id));
                }          
            }
            return Ok(StatusCode(StatusCodes.Status202Accepted, "Upload successfully"));
        }
        [HttpDelete("delete-pictures/{id}")]
        public async Task<IActionResult> DeletePictureAsync([FromRoute]Guid id, List<string> pictures)
        {
            try
            {
                await _productService.DeleteImage(productId: id, pictures);
            }
            catch (KeyNotFoundException)
            {
                return Ok(StatusCode(StatusCodes.Status404NotFound, "No product with Id: " + id));
            }
            catch (FileNotFoundException)
            {
                return Ok(StatusCode(StatusCodes.Status404NotFound, "Cannot delete image from azure storage due to file not found!"));
            }
            catch(Exception ex)
            {
                return Ok(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }            
            return Ok(StatusCode(StatusCodes.Status202Accepted, "Delete successfully"));
        }

        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                _productService.Add(product);
            } catch (DuplicateNameException)
            {
                return Ok(new CustomResponse(409));
            }
            return Ok(new CustomResponse(201));
        }

        // PUT api/<ProductController>/5
        [HttpPut]
        public IActionResult Put([FromBody]Product product)
        {
            try
            {                
                _productService.Update(product);
            }
            catch (DuplicateNameException)
            {
                return Ok(new CustomResponse(409));
            }
            return Ok(new CustomResponse(202));
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _productService.Disable(id);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new CustomResponse(404));
            }
            return Ok(new CustomResponse(200));
        }
    }
}
