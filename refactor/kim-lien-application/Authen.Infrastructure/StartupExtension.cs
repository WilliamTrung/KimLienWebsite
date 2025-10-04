using Common.Domain.Entities;
using Common.Kernel.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Authen.Infrastructure
{
    public static class StartupExtension
    {
        private static List<string> _roles = new List<string> { Roles.Default, Roles.Administrator };
        private static List<string> _adminEmails = new List<string> { "williamthanhtrungq2@gmail.com" };
        private static string _defaultPassword = "Pass@123";
        public static async Task ScanAdministrator(this IServiceProvider services)
        {
            await SeedRolesAsync(services);
            await SeedDefaultUserAsync(services);
        }
        private static async Task SeedDefaultUserAsync(this IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            foreach (var email in _adminEmails)
            {
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    DisplayName = email,
                    PhoneNumber = "0908941761",
                    Region = "vn"
                };
                await userManager.CreateAsync(user, _defaultPassword);
                await userManager.AddToRolesAsync(user, new List<string> { Roles.Administrator });
            }
        }
        private static async Task SeedRolesAsync(this IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<Role>>();

            foreach (var roleName in _roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Role
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpperInvariant()
                    });
                }
            }
        }
    }
}
