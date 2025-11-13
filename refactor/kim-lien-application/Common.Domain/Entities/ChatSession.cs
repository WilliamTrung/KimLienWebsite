using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ChatSession : BaseEntity<Guid>, IAuditEntity
    {
        public Guid RoomId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }

        [ForeignKey(nameof(RoomId))]
        public ChatRoom Room { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
