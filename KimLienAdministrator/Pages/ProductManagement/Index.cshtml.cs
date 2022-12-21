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

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var products = await _unitOfWork.ProductService.GetDTOs();
            Product = products.ToList();
        }
    }
}
