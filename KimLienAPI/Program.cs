using AppCore;
using ApiService.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using ApiService.ServiceAdministrator;
using ApiService.ServiceAdministrator.Implementation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ApiService.Azure;
using ApiService;
using JwtService.Models;
using JwtService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

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
builder.Services.AddAuthorization();
var services = builder.Services;
services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

services.AddDbContext<SqlContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var jwtConfig = builder.Configuration.GetSection("JwtConfiguration");
if(jwtConfig != null)
{
    Console.WriteLine("Issuer: " + jwtConfig["Issuer"]);
    Console.WriteLine("Audience: " + jwtConfig["Audience"]);
    Console.WriteLine("Key:" + jwtConfig["Key"]);
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"])),
            
        };
    });
} else
{
    throw new Exception("Cannot initialize jwt bearer");
}
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));
services.AddAutoMapper(typeof(Mapping));
services.TryAddScoped<IJwtService, JwtService.Implementation.JwtService>();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("CorsPolicy");
app.Run();
