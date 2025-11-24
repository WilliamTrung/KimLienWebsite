using AppCore;
using AppService;
using AppService.IService;
using AppService.Service;
using AppService.UnitOfWork;
using KimLienAdministrator;
using KimLienAdministrator.Helper.Azure.Blob;
using KimLienAdministrator.Helper.Azure.IBlob;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // This is the line you would change if your configuration files were somewhere else
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
var services = builder.Services;
services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
services.AddControllers().AddNewtonsoftJson
               (
                   x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               );
services.AddDbContext<SqlContext>(option => option.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddSession();
services.AddAutoMapper(typeof(Mapping));
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IProductBlob, ProductBlob>();
services.AddTransient<PictureBlob>();
services.AddSession(option => option.IdleTimeout = TimeSpan.FromMinutes(30));
services.AddTransient<IAuthService, AuthService>();

var blobStorage = configuration.GetSection("BlobStorage");
var connection = blobStorage["AzureWebJobsStorage"];

var app = builder.Build();

app.UsePathBase("/Administrator");
Startup.CreateDBAsync(app).Wait();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
//add session
app.UseSession();
app.Run();
