using AppService.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimLienAdministrator.Pages
{
    public class LogoutModel : PageModel
    {
        public RedirectToPageResult OnGet()
        {
            HttpContext.Session.Logout();
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
