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

namespace KimLienCustomerView.Pages.ProductView
{
    public class ProductsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<ProductModel> Products { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _unitOfWork.ProductService.GetProductModels();          
            Products = result.ToList();
        }
    }
}
