using AppCore;
using AppService;
using AppService.DTOs;
using AppService.IService;
using AppService.Service;
using AppService.UnitOfWork;
using Azure.Storage.Blobs;
using KimLienAdministrator;
using KimLienAdministrator.Helper.Azure.Blob;
using KimLienAdministrator.Helper.Azure.IBlob;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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
services.AddDbContext<SqlContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddSession();
services.AddAutoMapper(typeof(Mapping));
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IProductBlob, ProductBlob>();
services.AddSession(option => option.IdleTimeout = TimeSpan.FromMinutes(30));
services.AddTransient<IAuthService, AuthService>();

using(var _config = builder.Configuration)
{
    var blobStorage = _config.GetSection("BlobStorage");
    var connection = blobStorage["AzureWebJobsStorage"];
}

var app = builder.Build();

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
