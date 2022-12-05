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

        public IList<ProductModel> Products { get;set; } = default!;
        
        public async Task OnGetAsync()
        {
            var result = await _unitOfWork.ProductService.GetProductModels();          
            Products = result.ToList();

            Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://cdn.discordapp.com/attachments/694099165004955712/1045745170047582248/316162893_5250672098369953_520216915754902718_n.jpg",
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });Products.Add(new ProductModel()
            {
                Product = new AppService.DTOs.Product()
                {
                    Name = "123",
                    Pictures = "https://media.tenor.com/zGW3B-JOrNwAAAAC/klee-genshin.gif",
                    Id = Guid.NewGuid(),
                    Description = "blah blah blah boy i am god of war you mother fucker"
                }

            });
        }
        private void AdjustModels()
        {
            if(Products != null)
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
