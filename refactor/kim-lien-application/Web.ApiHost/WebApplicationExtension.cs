using Common.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web.ApiHost
{
    public static class WebApplicationExtension
    {
        public static void EnsureDatabaseMigrated(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var globalDbContext = services.GetRequiredService<CentralData.MigrateDbContext.GlobalDbContext>();
                globalDbContext.Database.Migrate();
                Task.Run(() => SeedRolesAsync(services));
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<WebApplication>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }
        }
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (var roleName in new[] { "Default", "Administrator" })
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
