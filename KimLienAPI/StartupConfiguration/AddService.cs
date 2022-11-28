using AppService.IService;
using AppService.Service;
using AppService.UnitOfWork;

namespace KimLienAPI.StartupConfiguration
{
    public static class AddService
    {
        public static void Add(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}
