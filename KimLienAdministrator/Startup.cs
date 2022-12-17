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
                    Role role = new Role()
                    {
                        Name = "Administrator"
                    };
                    role = await unitOfWork.RoleService.Create(role);
                    User administrator = new User()
                    {
                        RoleId = role.Id,
                        Password = "Kimlien$1966"
                    };
                    administrator = await unitOfWork.UserService.Create(administrator);
                    Console.WriteLine(administrator.Id);
                    Console.WriteLine(role.Name);
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
