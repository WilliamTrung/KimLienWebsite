using Common.Api;
using Common.DomainException.Middleware;
using Common.Extension.Jwt;
using Common.Infrastructure.ProductViewService;
using Common.Infrastructure.Storage.Azure;
using Common.Logging.Middleware;
using Common.RequestContext;
using Common.RequestContext.Abstractions;
using Common.RequestContext.Middleware;
using Common.TaskHolder.Abstractions;
using Common.TaskHolder.Middleware;
using Common.TaskHolder.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Web.ApiHost;

var builder = WebApplication.CreateBuilder(args);
// I. Configs builder
// 1. Set base configuration path
var basePath = Environment.GetEnvironmentVariable("BASE_PATH");
var appSetting = Environment.GetEnvironmentVariable("APP_SETTING");

#if DEBUG
basePath ??= Directory.GetCurrentDirectory();
appSetting ??= "appsettings.json";
#endif

if (string.IsNullOrWhiteSpace(basePath))
    throw new InvalidOperationException("BASE_PATH is required in non-DEBUG.");

if (string.IsNullOrWhiteSpace(appSetting))
    throw new InvalidOperationException("APP_SETTING is required in non-DEBUG.");

// Clean the current configuration
builder.Configuration.Sources.Clear();

// Point to new appsetting folder
var provider = new PhysicalFileProvider(basePath);
var env = builder.Environment;
builder.Configuration
    .AddJsonFile(provider, appSetting, optional: false, reloadOnChange: true)
    .AddJsonFile(provider, $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);
// 2. Serilog
builder.Host.UseSerilog((ctx, logCfg) => logCfg.ReadFrom.Configuration(builder.Configuration));
// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IRequestContext, RequestContext>();
builder.Services.AddScoped<RequestContextMiddleware>();
builder.Services.AddScoped<RequestLoggingMiddleware>();
builder.Services.AddScoped<DomainExceptionMiddleware>();
builder.Services.AddScoped<TaskHolderMiddleware>();
builder.Services.AddScoped<ITaskHolder, TaskHolder>();
builder.Services.RegisterProductViewService(builder.Configuration);
builder.Services.AddAzureBlobStorage(builder.Configuration);
//Add configuration as IOptions here
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddSingleton(_ => new Npgsql.NpgsqlConnection(connectionString));
//++

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

var moduleAssemblies = AssemblyHelper.GetModuleAssemblies();
builder.Services.AddModuleControllers(moduleAssemblies);
builder.Services.AddModuleDI(builder.Configuration, moduleAssemblies);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
var app = builder.Build();
app.EnsureDatabaseMigrated();
// IMPORTANT: order
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// CORS should be after routing, before auth/authorization
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Put Swagger before custom middlewares to avoid them intercepting UI/static files.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Keep it under /swagger/ (avoid conflicting with root)
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

// Custom middlewares AFTER Swagger so they don't swallow swagger assets
app.UseMiddleware<DomainExceptionMiddleware>();
app.UseMiddleware<RequestContextMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<TaskHolderMiddleware>();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();