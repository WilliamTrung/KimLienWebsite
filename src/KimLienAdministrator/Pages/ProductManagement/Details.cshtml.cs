using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Entities;
using AppService.UnitOfWork;
using AppService.Models;
using AppService;

namespace KimLienAdministrator.Pages.ProductManagement
{
    [Authorized("Administrator")]
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public ProductModel Product { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return RedirectToPage("/ProductManagement/Index");
            }

            var find = await _unitOfWork.ProductService.GetProductModels(filter: p => p.Id == id);
            var found = find.FirstOrDefault();
            if (found == null)
            {
                return RedirectToPage("/ProductManagement/Index");
            }
            else 
            {
                Product = found;
            }
            return Page();
        }
    }
}
