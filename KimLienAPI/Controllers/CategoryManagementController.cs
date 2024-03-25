using KimLienAPI.Helper;
using KL_Service.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models.Enum;
using Models.ServiceModels.Categories.Operation;

namespace KimLienAPI.Controllers
{
    [Authorize(Role.Administrator)]
    [Route("api/management/category")]
    [ApiController]
    public class CategoryManagementController : ODataController
    {
        private readonly ICategoryManagementService _categoryManagementService;
        public CategoryManagementController(ICategoryManagementService categoryManagementService)
        {
            _categoryManagementService = categoryManagementService;
        }
        [HttpGet]
        [EnableQuery]
        public IActionResult GetCategories()
        {
            var result = _categoryManagementService.GetCategories();
            return Ok(result);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCategoryById(Guid id)
        {
            var result = _categoryManagementService.GetCategory(id);
            return Ok(result);
        }
        [HttpGet]
        [Route("children/{parentId}")]
        public IActionResult GetChildren(Guid parentId)
        {
            var result = _categoryManagementService.GetChildren(parentId);
            return Ok(result);
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCategory(CategoryAddModel category)
        {
            var createdId = await _categoryManagementService.AddCategory(category);
            return Created(createdId);
        }
        [HttpPatch]
        [Route("modify")]
        public async Task<IActionResult> ModifyCategory(CategoryModifyModel category)
        {
            await _categoryManagementService.ModifyCategory(category);
            return NoContent();
        }
        [HttpPatch]
        [Route("toggle")]
        public async Task<IActionResult> ToggleCategoryStatus(Guid id)
        {
            await _categoryManagementService.ToggleCategoryStatus(id);
            return NoContent();
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryManagementService.DeleteCategory(id);
            return NoContent();
        }
    }
}
