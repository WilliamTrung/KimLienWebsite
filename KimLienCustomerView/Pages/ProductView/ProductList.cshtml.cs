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
using AppService.Paging;
using AppService.DTOs;

namespace KimLienCustomerView.Pages.ProductView
{
    public class ProductList : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public string Truncate(string proName, int maxChars, int maxCharShown)
        {
            if (proName.Length >= maxCharShown)
            {
                return proName.Length <= maxChars ? proName : proName.Substring(0, maxChars) + "...";
            }
            return proName;
        }

        public ProductList(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<ProductModel> Products { get; set; } = default!;
        [BindProperty]
        public int MaxPages { get; set; } = 0;
        [BindProperty]
        public int PageIndex { get; set; } = 0;

        private int PageSize { get; set; } = 12;
        public async Task OnPostAsync(int? index = 0)
        {
            if (index != null)
            {
                PageIndex = (int)index;
            }
            await OnGetAsync();
        }
        public async Task OnGetAsync(string? name = null, string? category = null)
        {
            int total = await _unitOfWork.ProductService.GetTotal();
            MaxPages = (int)Math.Ceiling((double)total / PageSize);
            var page = new PagingRequest()
            {
                PageSize = PageSize,
                PageIndex = PageIndex
            };
            IEnumerable<ProductModel> result = await _unitOfWork.ProductService.GetProductModels(paging: page);
            if (category != null)
            {
                result = result.Where(e => e.ProductCategories.Any(c => c.Category.Name == category));
                ViewData["Category"] = category;
            }
            if(name != null)
            {
                result = result.Where(e => e.Product.Name.ToLower().Contains(name.ToLower()));
            }
            Products = result.ToList();
            
        }
        private void AdjustModels()
        {
            if (Products != null)
            {
                foreach (var product in Products)
                {
                    var productName = product.Product.Name;
                    int maxChars = 50;
                    int maxCharShown = 47;
                    product.Product.Name = Truncate(productName, maxChars, maxCharShown);
                }
            }
        }
    }
}
