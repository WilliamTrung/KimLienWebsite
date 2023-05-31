using AppCore;
using ApiService.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using System.Reflection.Emit;
using ApiService.ServiceAdministrator;
using ApiService.ServiceAdministrator.Implementation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ApiService.Azure;
using ApiService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        //.AddRouteComponents(
        //    "odata",
        //    KimLienAPI.Startup.ODataConfiguring.GetEdmModel())
        )
    .AddNewtonsoftJson
               (
                   x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var services = builder.Services;
services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

services.AddDbContext<SqlContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
services.AddAutoMapper(typeof(Mapping));
services.AddTransient<IUnitOfWork, UnitOfWork>();
services.AddTransient<IAzureService, AzureService>();
services.AddTransient<IProductService, ProductService>();
services.AddTransient<ICategoryService, CategoryService>();
services.AddTransient<IProductCategoryService, ProductCategoryService>();
services.AddTransient<IRoleService, RoleService>();
services.AddTransient<IUserService, UserService>();
services.AddTransient<ApiService.ServiceCustomer.IProductService, ApiService.ServiceCustomer.Implementation.ProductService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.UseCors("CorsPolicy");
app.Run();
