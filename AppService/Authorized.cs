using AppService.DTOs;
using AppService.Extension;
using AppService.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppService
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class Authorized : Attribute, IAsyncPageFilter
    {
        List<string> roles;
        private IUnitOfWork _unitOfWork = null!;
        public Authorized(string roles) {
            this.roles = roles.Split(",").ToList();
        }
        public Authorized()
        {
            roles = new List<string>();
        }
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            await next.Invoke();
        }
        private async Task<bool> AuthorizingAsync(User user)
        {
            bool result = false;
            //check user in db
            var find = await _unitOfWork.UserService.GetDTOs(filter: u => u.Id== user.Id, includeProperties: "Role");
            var found = find.FirstOrDefault();
            if(found != null)
            {
                //in db --> check role
                var role = found.Role;
                if(role != null)
                {
                    var check = roles.Any(r => r == role.Name);
                    if (check)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
         async Task IAsyncPageFilter.OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            var services = context.HttpContext.RequestServices;
            var requested_unitOfWork = services.GetService(typeof(IUnitOfWork));
            if (requested_unitOfWork != null)
            {
                _unitOfWork = (IUnitOfWork)requested_unitOfWork;
                var login = context.HttpContext.Session.GetLoginUser();
                bool IsAuthorized = false;
                if (login != null)
                {
                    IsAuthorized = await AuthorizingAsync(login);
                }
                if (!IsAuthorized)
                {
                    context.HttpContext.Response.Redirect("/");
                }
            } 
            await Task.CompletedTask;
        }
    }
}
