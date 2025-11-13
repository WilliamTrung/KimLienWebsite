using Common.Kernel.KafkaService.Models;

namespace Common.Kernel.KafkaService.Abstractions
{
    public interface IKafkaProducer
    {
        Task<bool> PostMessage(string topic, MessageDto message);
        Task PublishMessage<T>(string topic, string key, T message);
    }
}
