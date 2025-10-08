using Azure.Storage.Blobs;
using Common.Application.Storage.Abstraction;
using Common.Infrastructure.Storage.Azure.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure.Storage.Azure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureBlobStorage(
        this IServiceCollection services,
        IConfiguration config,
        string sectionName = "AzureBlob")
        {
            services.Configure<AzureBlobOption>(config.GetSection(sectionName));

            services.AddSingleton(provider =>
            {
                var opt = provider.GetRequiredService<IOptions<AzureBlobOption>>().Value;
                return new BlobServiceClient(opt.ConnectionString);
            });

            services.AddKeyedSingleton<ICloudStorageService, AzureStorageService>(nameof(AzureStorageService));
            return services;
        }
    }
}
