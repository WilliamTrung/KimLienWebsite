using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Entities;
using KimLienAdministrator.Helper.Azure;
using KimLienAdministrator.Helper.Azure.IBlob;

namespace KimLienAdministrator.Pages.ProductManagement
{
    public class IndexModel : PageModel
    {
        private readonly AppCore.SqlContext _context;
        private readonly IProductBlob _productBlob;

        public IndexModel(AppCore.SqlContext context, IProductBlob productBlob)
        {
            _context = context;
            _productBlob = productBlob;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products.ToListAsync();
                var product = Product.FirstOrDefault();
                if(product != null)
                {
                    var check = _productBlob.DeleteAsync(product.Id, product.Pictures.Split(",")[0]);
                }
            }
        }
    }
}
