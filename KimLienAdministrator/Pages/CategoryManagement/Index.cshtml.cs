using AppService;
using AppService.DTOs;
using AppService.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimLienAdministrator.Pages.CategoryManagement
{
    [Authorized("Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IList<Category> Category { get;set; } = default!;
        public CategoryMessage Message { get; set; } = default!;
        public async Task OnGetAsync()
        {
            await GetCategoriesAsync();

            
        }
        private async Task GetCategoriesAsync()
        {
            var result = await _unitOfWork.CategoryService.GetDTOs();
            if(Message == null)
            {
                Message = new CategoryMessage();
            }
            Category = result.ToList();
        }

        public async Task<IActionResult> OnPostAsync(Guid id, string name, string action)
        {
            if(action == "update")
            {
                var find = await _unitOfWork.CategoryService.GetDTOs(c => c.Id == id);
                var found = find.FirstOrDefault();
                if (found != null && found.Name != name)
                {
                    found.Name = name;
                    if (TryValidateModel(found))
                    {
                        var result = await _unitOfWork.CategoryService.Update(filter: c => c.Id == found.Id, found);
                    }
                    else
                    {
                        if (Message == null)
                        {
                            Message = new CategoryMessage();
                        }
                        Message.Id = found.Id;
                        Message.Message = Helper.Strings.CategoryUpdateFailMessage;
                    }
                }
            } else if(action == "delete") {
                var find = await _unitOfWork.CategoryService.GetDTOs(c => c.Id == id);
                var found = find.FirstOrDefault();
                if(found != null)
                {
                    var result = await _unitOfWork.CategoryService.Delete(found);
                    if(result == false)
                    {
                        //delete failed
                        if (Message == null)
                        {
                            Message = new CategoryMessage();
                        }
                        Message.Id = found.Id;
                        Message.Message = Helper.Strings.CategoryDeleteFailMessage;
                    }
                } 
            }
            
            await GetCategoriesAsync();
            return Page();
        }
    }
}
