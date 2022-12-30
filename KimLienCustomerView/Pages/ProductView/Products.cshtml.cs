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

namespace KimLienCustomerView.Pages.ProductView
{
    public class ProductsModel : PageModel
    {
        private static Random rng = new Random();
        private readonly IUnitOfWork _unitOfWork;
        public string Truncate(string proName, int maxChars, int maxCharShown)
        {
            if (proName.Length >= maxCharShown)
            {
                return proName.Length <= maxChars ? proName : proName.Substring(0, maxChars) + "...";
            }
            return proName;
        }

        public ProductsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<ProductModel> NewProducts { get;set; } = default!;
        public IList<ProductModel> HotProducts { get; set; } = default!;
        public IList<string> BankPics { get; set; } = new List<string>()
        {
            "https://www.tcqtkd.edu.vn/wp-content/uploads/2022/09/gio-lam-viec-agribank-2.jpg",
            "https://cdn.tgdd.vn/2020/04/GameApp/unnamed-200x200-18.png",
            "https://vanhoadoisong.vn/wp-content/uploads/2022/08/vpbank-la-ngan-hang-gi-ngan-hang-vpbank-co-uy-tin-khong-nhung-san-pham-dich-vu-vpbank-03.jpg",
            "https://i.redd.it/5nd6lxvfnp161.jpg"
        };
        
        public async Task OnGetAsync()
        {
            await SetNewProductsAsync();
            await SetHotProductsAsync();
        }
        private async Task SetNewProductsAsync()
        {
            var products = await _unitOfWork.ProductService.GetProductModels();
            products = products.OrderByDescending(p => p.Product.ModifiedDate);
            NewProducts = products.ToList();
        }
        private async Task SetHotProductsAsync()
        {
            var products = await _unitOfWork.ProductService.GetProductModels();
            products = products.OrderBy(p => rng.Next()).ToList();
            HotProducts = products.ToList();
        }
        private void AdjustModels()
        {
            //if(Products != null)
            //{
            //    foreach (var product in Products)
            //    {
            //        var productName = product.Product.Name;
            //        int maxChars = 50;
            //        int maxCharShown = 47;
            //        product.Product.Name = Truncate(productName, maxChars, maxCharShown);
            //    }
            //}
        }
    }
}
