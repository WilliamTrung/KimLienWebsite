using Common.Api;
using Common.DomainException.Middleware;
using Common.Logging.Middleware;
using Common.RequestContext.Middleware;
using Microsoft.Extensions.FileProviders;
using Serilog;

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

builder.Services.AddHttpClient();
//Add configuration as IOptions here
//++

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

var moduleAssemblies = AssemblyHelper.GetModuleAssemblies();
builder.Services.AddModuleControllers(moduleAssemblies);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();
var app = builder.Build();


app.UseCors(builder => builder
  .SetIsOriginAllowed(origin => true)
  .AllowAnyMethod()
  .AllowAnyHeader()
  .AllowCredentials());

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<RequestrContextMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<DomainExceptionMiddleware>();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.MapControllers();
app.Run();
