using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppCore;
using AppService.UnitOfWork;
using AppService.DTOs;
using Microsoft.AspNetCore.Authorization;
using AppService;

namespace KimLienAdministrator.Pages.CategoryManagement
{
    [Authorized("Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await InitAsync();
            return Page();
        }

        public async Task InitAsync()
        {
            var mainCategories = await _unitOfWork.CategoryService.GetDTOs(filter: c => c.ParentId == null);
            mainCategories = mainCategories.Prepend(new Category()
            {
                Name= "Không có",
                Id = Guid.Empty
            });
            MainCategories = new MultiSelectList(mainCategories, "Id", "Name");
        }
        [BindProperty]
        public Category Category { get; set; } = null!;
        public MultiSelectList MainCategories { get; set; } = null!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(Guid? CategoryId)
        {
          if (!ModelState.IsValid)
            {
                await InitAsync();
                return Page();
            }
          if(CategoryId != Guid.Empty)
            {
                Category.ParentId = CategoryId;
            }
            var result = await _unitOfWork.CategoryService.Create(Category);

            return RedirectToPage("./Index");
        }
    }
}
