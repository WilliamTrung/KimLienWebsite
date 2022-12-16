using AppService.DTOs;
using AppService.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimLienAdministrator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public new User User { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            var login = await _unitOfWork.UserService.Login(User);
            if (login == null)
            {
                Message = Helper.Strings.LoginFailed;
                return Page();
            } else
            {
                return RedirectToPage("/ProductManagement");
            }
        }
    }
}