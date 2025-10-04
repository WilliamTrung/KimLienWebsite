using Authen.Infrastructure;
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
                //services.ScanAdministrator().Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<WebApplication>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }
        }
    }
}
