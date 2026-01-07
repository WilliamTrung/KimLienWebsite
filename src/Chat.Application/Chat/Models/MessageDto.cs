using Chat.Application.Common.Models;

namespace Chat.Application.Chat.Models
{
    public class MessageDto
    {
        public string? Id { get; set; }
        public string Message { get; set; } = null!;
        public string RoomId { get; set; } = null!;
        public string? SenderId { get; set; }
        public UserDto? Sender { get; set; }
        public DateTime SentAt { get; set; }
        public object? Metadata { get; set; }
    }
}
