using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using KimLienAdministrator.Helper.Azure;
using KimLienAdministrator.Helper.Azure.IBlob;
using AppService.UnitOfWork;
using AppService.DTOs;
using AppService;
using Microsoft.CodeAnalysis;
using KimLienAdministrator.Pages.CategoryManagement;
using AppService.Models;

namespace KimLienAdministrator.Pages.ProductManagement
{
    [Authorized("Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IProductBlob _productBlob;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IProductBlob productBlob, IUnitOfWork unitOfWork)
        {
            _productBlob = productBlob;
            _unitOfWork = unitOfWork;
        }

        public IList<Product> Products { get; set; } = default!;

        public async Task OnGetAsync(string? name = "")
        {
            await GetProductAsync(name);
        }
        private async Task GetProductAsync(string? name = "")
        {
            var result = await _unitOfWork.ProductService.GetDTOs();
            if (name != string.Empty && name !=null)
            {
                var search = result.Where(c => c.Name.ToLower().Contains(name.ToLower()));
                result = search;
            }
            Products =  result.ToList();
        }
    }
}
