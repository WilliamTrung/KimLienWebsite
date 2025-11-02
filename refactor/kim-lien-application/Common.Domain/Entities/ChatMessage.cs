using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Common.Domain.Entities
{
    public class ChatMessage : BaseEntity<Guid>
    {
        public Guid RoomId { get; set; }
        public Guid? SenderId { get; set; }
        public string Message { get; set; } = null!;
        public JsonDocument? Metadata { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public string IpAddress { get; set; } = null!;
        public Guid? EditFromMessageId { get; set; }
        public Guid? ReplyToMessageId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User? Sender { get; set; }
        [ForeignKey(nameof(ReplyToMessageId))]
        public ChatMessage? ReplyToMessage { get; set; }
        [ForeignKey(nameof(EditFromMessageId))]
        public ChatMessage? EditFromMessage { get; set; }
        [ForeignKey(nameof(RoomId))]
        public ChatRoom Room { get; set; } = null!;

    }
}
