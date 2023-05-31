using ApiService.DTOs;
using ApiService.ServiceAdministrator;
using KimLienAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KimLienAPI.Controllers.Administrator
{
    [Route("api/administrator/categories")]
    [ApiController]
    public class CategoryController : ODataController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var result = _categoryService.GetCategories();
            return Ok(result);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            try
            {
                _categoryService.Add(category);
            } catch(DuplicateNameException)
            {
                return Ok(StatusCode(409, "Duplicated category name"));
            }
            return Ok(StatusCode(201));

        }

        // PUT api/<CategoryController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Category category)
        {
            try
            {
                _categoryService.Update(category);
            }
            catch (DuplicateNameException)
            {
                return Ok(StatusCode(409, "Duplicated category name"));
            }
            return Ok(StatusCode(201));
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _categoryService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new CustomResponse(404));
            }
            return Ok(new CustomResponse(200));
        }
    }
}
