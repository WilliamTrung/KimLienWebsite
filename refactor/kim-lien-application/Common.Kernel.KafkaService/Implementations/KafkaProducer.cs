using Common.Extension;
using Common.Kernel.Dependencies;
using Common.Kernel.KafkaService.Abstractions;
using Common.Kernel.KafkaService.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Kernel.KafkaService.Implementations
{
    public class KafkaProducer : IKafkaProducer, IDisposable, IScoped
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;
        private readonly KafkaSetting _kafkaSetting;
        public KafkaProducer(ILogger<KafkaProducer> logger, IOptions<KafkaSetting> setting) : base()
        {
            _logger = logger;
            var config = CreateConfig();
            _producer = new ProducerBuilder<string, string>(config).Build();
            _kafkaSetting = setting.Value;
        }

        public void Dispose()
        {
            if (_producer != null)
            {
                _producer.Flush(TimeSpan.FromSeconds(10));
                _producer.Dispose();
            }
        }

        public async Task PublishMessage<T>(string topic, string key, T message)
        {
            try
            {
                var x = message.TrySerializeObjectWithSystemLibrary();
                var result = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = key,
                    Value = message.TrySerializeObjectWithSystemLibrary()
                });
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Message delivery failed: {ex.Error.Reason}");
                _logger.LogError(ex, $"Message delivery failed{Environment.NewLine}topic: {topic}{Environment.NewLine}message: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message delivery failed: {ex.Message}");
                _logger.LogError(ex, $"Message delivery failed{Environment.NewLine}topic: {topic}{Environment.NewLine}message: {message}");
            }
        }

        public async Task<bool> PostMessage(string topic, MessageDto message)
        {
            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = message.Key,
                    Value = message.Message
                });

                return result.Status != PersistenceStatus.NotPersisted;
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Message delivery failed: {ex.Error.Reason}");
                _logger.LogError(ex, $"Message delivery failed{Environment.NewLine}topic: {topic}{Environment.NewLine}message: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message delivery failed: {ex.Message}");
                _logger.LogError(ex, $"Message delivery failed{Environment.NewLine}topic: {topic}{Environment.NewLine}message: {message}");
            }

            return false;
        }

        private ProducerConfig CreateConfig()
        {
            Enum.TryParse<SaslMechanism>(_kafkaSetting.Mechanism, true, out var _mechanism);
            Enum.TryParse<Acks>(_kafkaSetting.Acks, true, out var _acks);
            Enum.TryParse<CompressionType>(_kafkaSetting.Acks, true, out var _compressionType);
            Enum.TryParse<SecurityProtocol>(_kafkaSetting.SecurityProtocol, true, out var _securityProtocol);

            return new ProducerConfig()
            {
                BootstrapServers = _kafkaSetting.BootstrapServers,
                SecurityProtocol = _securityProtocol,
                SaslMechanism = _mechanism,
                SaslUsername = _kafkaSetting.Username,
                SaslPassword = _kafkaSetting.Password,
                Acks = _kafkaSetting.EnableIdempotence ? Acks.All : _acks,
                EnableIdempotence = _kafkaSetting.EnableIdempotence,
                CompressionType = _compressionType,
                LingerMs = _kafkaSetting.LingerMs,
                BatchSize = _kafkaSetting.BatchSize
            };
        }
    }
}
