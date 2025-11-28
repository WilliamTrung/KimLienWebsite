namespace Chat.Application.Chat.Models
{
    public class MessageDto
    {
        public string Message { get; set; } = null!;
        public string RoomId { get; set; } = null!;
        public object? Metadata { get; set; }
    }
}
