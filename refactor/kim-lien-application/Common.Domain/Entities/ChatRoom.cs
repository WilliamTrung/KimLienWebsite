using Common.Infrastructure.Interceptor.TenantQuery.Model;
using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;

namespace Common.Domain.Entities
{
    public class ChatRoom : BaseEntity<Guid>, ITenantEntity, IAuditEntity
    {
        public List<ChatMessage> Messages { get; set; } = new();
        public string TenantId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPersonal { get; set; }
        public virtual ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}
