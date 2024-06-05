using KimLienAPI.Startup;
using KL_Core;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.EnableQueryFeatures();
    });
builder.Services.AddDbContext<KimLienContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Database:Default"]);
});
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(KL_SP_MappingConfig.AutoMapperAssembly)));
builder.JwtConfiguration();
builder.InjectUnitOfWork();
builder.InjectAzureService();
builder.AddCustomerFeature();
builder.AddManagementFeature();
builder.AddCustomerService();
builder.AddManagementService();
builder.AddAuthFeature();
builder.AddAuthService();

builder.ConfigPolicy(PolicyConfiguration.LocalPolicy);
builder.ConfigPolicy(PolicyConfiguration.AnonymousPolicy);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(PolicyConfiguration.AnonymousPolicy);
app.MapControllers();
app.Services.EnsureSqlDatabase();
app.Run();
