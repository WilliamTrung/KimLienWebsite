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

        public ProductModel Product { get; set; }
        
        public IList<ProductModel> recommendProduct { get; set; } = default!;

        public async Task OnGetAsync(Guid? id)
        {
            var find = await _unitOfWork.ProductService.GetProductModels();
            var found = find.FirstOrDefault();
            if (found != null)
            {
                Product = found;
            }

        }
        private void AdjustModels()
        {
            if (recommendProduct != null)
            {
                foreach (var product in recommendProduct)
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
