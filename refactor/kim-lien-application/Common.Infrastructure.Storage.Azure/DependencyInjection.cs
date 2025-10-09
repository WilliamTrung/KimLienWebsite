using Azure.Storage.Blobs;
using Common.Application.Storage.Abstraction;
using Common.Infrastructure.Storage.Azure.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Common.Infrastructure.Storage.Azure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureBlobStorage(
        this IServiceCollection services,
        IConfiguration config,
        string sectionName = "AzureBlob",
        string profileName = "Profiles")
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            var profilesSection = config.GetSection($"{sectionName}:{profileName}");
            foreach (var child in profilesSection.GetChildren())
            {
                string name = child.Key; // "products", "avatars", ...
                services
                    .AddOptions<AzureBlobProfile>(name)
                    .Bind(child)
                    .Validate(opt => !string.IsNullOrWhiteSpace(opt.ConnectionString) &&
                                     !string.IsNullOrWhiteSpace(opt.Container),
                              "Blob options are invalid");
                services.AddSingleton(provider =>
                {
                    var opt = provider.GetKeyedService<IOptions<AzureBlobProfile>>(name)?.Value;
                    if (opt == null)
                        throw new InvalidOperationException($"Azure Blob Storage profile '{name}' is not configured properly.");
                    return new BlobServiceClient(opt.ConnectionString);
                });
                services.AddKeyedSingleton<ICloudStorageService>(name, (sp, t) =>
                {
                    var opt = sp.GetRequiredService<IOptionsMonitor<AzureBlobProfile>>().Get(name);
                    var svcClient = new BlobServiceClient(opt.ConnectionString);
                    var logger = sp.GetRequiredService<ILogger<AzureStorageService>>();
                    return new AzureStorageService(svcClient, Options.Create(opt), logger);
                });
            }
            return services;
        }
    }
}
