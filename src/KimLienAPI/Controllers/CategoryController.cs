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
using AppService.Extension;

namespace KimLienAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;   
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _unitOfWork.CategoryService.GetDTOs();
            
            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{name}")]
        public async Task<ActionResult<Category>> GetCategory(string name)
        {
            var category = await _unitOfWork.CategoryService.GetDTOs(c => Helper.MinimalCompareString(c.Name, name));

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category.First());
        }
    }
}
