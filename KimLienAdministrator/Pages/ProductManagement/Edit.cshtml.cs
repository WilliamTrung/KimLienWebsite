using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppService.UnitOfWork;
using AppService.DTOs;

namespace KimLienAdministrator.Pages.ProductManagement
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public IList<ProductCategory> Categories { get; set; } = null!;
        public MultiSelectList SelectListItems { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var find =  await _unitOfWork.ProductService.GetDTOs(m => m.Id == id);
            if (find == null)
            {
                return RedirectToPage("./Index");
            }
            var found = find.FirstOrDefault();
            if(found == null)
            {
                return RedirectToPage("./Index");
            }
            Product = found;
            SetViewData();
            return Page();
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(Guid[] CategoryId)
        {
            var t = Categories;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            

            return RedirectToPage("./Index");
        }
        private async void SetViewData()
        {
            Categories = new List<ProductCategory>();
            var categories = await _unitOfWork.CategoryService.GetDTOs();
            SelectListItems = new MultiSelectList(categories, "Id", "Name");
        }
        public void AddCategory()
        {
            var category = new ProductCategory()
            {
                ProductId = Product.Id
            };
            Categories.Add(new ProductCategory());
        }
    }
}
