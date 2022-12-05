using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppCore;
using AppCore.Entities;

namespace KimLienAdministrator.Pages.ProductManagement
{
    public class CreateModel : PageModel
    {
        private readonly AppCore.SqlContext _context;

        public CreateModel(AppCore.SqlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        public List<IFormFile> Files { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //  {
            //      return Page();
            //  }

            //  _context.Products.Add(Product);
            //  await _context.SaveChangesAsync();

            var t = Files;
            return RedirectToPage("./Index");
        }
    }
}
