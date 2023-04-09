using AppService.UnitOfWork;
using CG.Web.MegaApiClient;
using KimLienAdministrator.Helper.Azure.Blob;
using KimLienAdministrator.Helper.Azure.IBlob;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimLienAdministrator.Pages
{
    public class UploadPictureModel : PageModel
    {
        private readonly IBlobService _blob;

        public UploadPictureModel(PictureBlob pictureBlob)
        {
            _blob = pictureBlob;
        }

        public IFormFile File { get; set; } = null!;
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (File != null)
            {
                //var check = await _blob.UploadAsync(File, "background_temp", "jpeg");
                //backround_3
                var check = await _blob.UploadAsync(File, "backround_3", "jpeg");
                if(check == false)
                {
                    ViewData["message"] = "Tải ảnh thất bại!";
                }
            } 


            ////testc set primary pic
            ////string test = "https://kimlien1808.blob.core.windows.net/products/fa53df43-bf29-4dbc-5495-08dad14a485b_14.png";
            ////var test1  = _unitOfWork.ProductService.SetPrimaryPicture(product.Id, test);


            return Page();
        }
    }
}
