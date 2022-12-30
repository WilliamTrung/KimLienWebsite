using AppCore.Entities;
using AppService.Models;
using AppService.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace KimLienCustomerView.Pages.ProductView
{
    public class ProductDetail : PageModel
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

        public ProductDetail(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductModel Product { get; set; } = null!;
        
        public IList<ProductModel> recommendProducts { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            var find = await _unitOfWork.ProductService.GetProductModels(filter: p => p.Id == id && p.IsDeleted == true);
            var found = find.FirstOrDefault();
            if (found != null)
            {
                Product = found;
                var products = await _unitOfWork.ProductService.GetProductModels();
                var relatives = products.Where(p => p.ProductCategories.Any(category => found.ProductCategories.Any(fCategory => fCategory.CategoryId == category.CategoryId)) && p.Product.Id != id);
                recommendProducts = relatives.ToList();
                return Page();
            } else
            {
                return RedirectToPage("MainPage");
            }

        }
        private void AdjustModels()
        {
            if (recommendProducts != null)
            {
                foreach (var product in recommendProducts)
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
