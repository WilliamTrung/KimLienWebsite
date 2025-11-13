namespace Common.Kernel.KafkaService.Models
{
    public class MessageDto
    {
        public string Key { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
