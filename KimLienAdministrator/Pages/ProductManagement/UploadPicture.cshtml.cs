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
            if(Files != null && Files.Count > 0)
            {
                var check = await _productBlob.UploadAsync(Files, product.Id);
            }
            

            //testc set primary pic
            //string test = "https://kimlien1808.blob.core.windows.net/products/fa53df43-bf29-4dbc-5495-08dad14a485b_14.png";
            //var test1  = _unitOfWork.ProductService.SetPrimaryPicture(product.Id, test);

            
             return Page();
        }
    }
}
