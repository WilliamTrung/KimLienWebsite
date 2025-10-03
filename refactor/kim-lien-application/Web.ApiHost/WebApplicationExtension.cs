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
                //var authenDbContext = services.GetRequiredService<Authen.Infrastructure.Data.AuthenIdentityDbContext>();
                //authenDbContext.Database.Migrate();
                //var adminDbContext = services.GetRequiredService<Admin.Infrastructure.Data.AdminDbContext>();
                //adminDbContext.Database.Migrate();
                var globalDbContext = services.GetRequiredService<CentralData.MigrateDbContext.GlobalDbContext>();
                globalDbContext.Database.Migrate();
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
