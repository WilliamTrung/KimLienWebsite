using KL_Service.AuthService;
using KL_Service.CategoryService;
using KL_Service.CustomerService;
using KL_Service.ProductService;
using KL_Service.StorageService;

namespace KimLienAPI.Startup
{
    public static class ServiceConfiguration
    {
        public static void AddManagementService(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IProductContainer, ProductContainer>();
            builder.Services.AddTransient<IProductManagementService, ProductManagementService>();
            builder.Services.AddTransient<ICategoryManagementService, CategoryManagementService>();
        }
        public static void AddCustomerService(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IProductCustomerService, ProductCustomerService>();
        }
        public static void AddAuthService(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IAuthService, AuthService>();
        }
    }
}
