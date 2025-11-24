using Common.Kernel.KafkaService.Abstractions;
using Common.Kernel.KafkaService.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Kernel.KafkaService
{
    public static class DependencyInjection
    {
        public static void RegisterKafka(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
        }
    }
}
