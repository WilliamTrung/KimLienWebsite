using ApiService.Adapter.Model;
using ApiService.ServiceAdministrator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KimLienAPI.Controllers.General
{
    [Route("api/category")]
    [ApiController]
    public class CategoryViewController : ODataController
    {
        private readonly ICategoryService _categoryService;
        public CategoryViewController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var list = _categoryService.GetCategories();
            return Ok(CategoryViewModelAdapter.FromCategoryDTOModels(list.ToList()));            
        }
    }
}
