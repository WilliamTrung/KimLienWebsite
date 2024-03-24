using KL_AuthFeature;
using KL_ManagementFeature;
using KL_ProductCustomerFeature;
using KL_Repository.UnitOfWork;

namespace KimLienAPI.Startup
{
    public static class FeatureConfiguration
    {
        public static void InjectUnitOfWork(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
        public static void AddManagementFeature(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IProductManagementFeature, ProductManagementFeature>();
            builder.Services.AddTransient<ICategoryManagementFeature, CategoryManagementFeature>();
        }
        public static void AddCustomerFeature(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IProductCustomerFeature, ProductCustomerFeature>();
        }
        public static void AddAuthFeature(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IAuthFeature, AuthFeature>();
        }
    }
}
