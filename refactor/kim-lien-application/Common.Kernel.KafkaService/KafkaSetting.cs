namespace Common.Kernel.KafkaService
{
    public class KafkaSetting
    {
        public string BootstrapServers { get; set; }
        public string Acks { get; set; }
        public bool EnableIdempotence { get; set; }
        public string CompressionType { get; set; }
        public int LingerMs { get; set; }
        public int BatchSize { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mechanism { get; set; }
        public string SecurityProtocol { get; set; }
    }
}
