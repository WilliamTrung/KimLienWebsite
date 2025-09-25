using AppCore;
using AppService.DTOs;
using AppService.UnitOfWork;

namespace KimLienAdministrator
{
    public static class Startup
    {
        public static async Task CreateDBAsync(WebApplication host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SqlContext>();
                    var unitOfWork = services.GetRequiredService<IUnitOfWork>();
                    context.Database.EnsureCreated();
                    var find_role = await unitOfWork.RoleService.GetDTOs(filter: r => r.Name == "Administrator");
                    var found_role = find_role.FirstOrDefault();
                    if(found_role == null)
                    {
                        Role role = new Role()
                        {
                            Name = "Administrator"
                        };
                        found_role = await unitOfWork.RoleService.Create(role);
                    }
                    var find_admin = await unitOfWork.UserService.GetDTOs(filter: u => u.RoleId == found_role.Id);
                    var found_admin = find_admin.FirstOrDefault();
                    if(found_admin == null)
                    {
                        User administrator = new User()
                        {
                            RoleId = found_role.Id,
                            Password = "Kimlien$1966"
                        };
                        found_admin = await unitOfWork.UserService.Create(administrator);
                    }
                    Console.WriteLine(found_admin.Id);
                    Console.WriteLine(found_role.Name);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured creating DB!");
                }
            }
        }
    }
}
