using Azure.Identity;
using Microsoft.Extensions.Azure;

namespace KimLienAPI.Startup
{
    public static class AzureConfiguration
    {
        public static void InjectAzureService(this WebApplicationBuilder builder)
        {
            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(builder.Configuration["Storage:Connection"]);
                clientBuilder.UseCredential(new DefaultAzureCredential());
            });
        }
    }
}
