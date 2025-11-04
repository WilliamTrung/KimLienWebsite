using Common.Domain.Entities;
using Common.Infrastructure.Interceptor.TenantQuery;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Data
{
    public class ChatContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyTenantQueryFilter();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserMetadata> UserMetadatas { get; set; }
    }
}
