using AppService.UnitOfWork;
using CG.Web.MegaApiClient;
using KimLienAdministrator.Helper.Azure.IBlob;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimLienAdministrator.Pages.ProductManagement
{
    public class UploadPictureModel : PageModel
    {
        private readonly IProductBlob _productBlob;
        private readonly IUnitOfWork _unitOfWork;

        public UploadPictureModel(IProductBlob productBlob, IUnitOfWork unitOfWork)
        {
            _productBlob = productBlob;
            _unitOfWork = unitOfWork;
        }

        public List<IFormFile> Files { get; set; } = null!;
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var products = await _unitOfWork.ProductService.GetDTOs();
            var product = products.FirstOrDefault();
            if(product == null)
            {
                return Page();
            }
            var check = await _productBlob.UploadAsync(Files, product.Id);
            return Page();
        }
    }
}
