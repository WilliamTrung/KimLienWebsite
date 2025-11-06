using Chat.Application.Chat.Abstractions;
using Chat.Application.Chat.Models;
using Chat.Infrastructure.Data;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.Extension;
using Common.RequestContext.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Chat.Implementations
{
    public class ChatService(IRequestContext requestContext
        , ChatContext chatContext
        , ILogger<ChatService> logger)
        : IChatService
    {
        public async Task SendMessage(MessageDto messageDto)
        {
            var userId = requestContext.Data.UserId!;
            // Here you would implement the logic to send the message,
            // for example, saving it to a database or sending it through a messaging system.
            var message = new ChatMessage
            {
                RoomId = Guid.Parse(messageDto.RoomId),
                SenderId = Guid.Parse(userId),
                Message = messageDto.Message,
                Metadata = messageDto.Metadata?.ToDocument(),
                IpAddress = requestContext.Data.IpAddress ?? throw new CException("IpAddress is null", System.Net.HttpStatusCode.BadRequest)
            };
            chatContext.Add(message);
            await chatContext.SaveChangesAsync();
        }
        public async Task<List<Guid>> GetUserInRoom(string roomId)
        {
            if (Guid.TryParse(roomId, out var roomGuid))
            {
                var sessions = await chatContext.ChatSessions.Where(x => x.RoomId == roomGuid && x.UserId.HasValue).ToListAsync(); 
                return sessions.Select(x => x.UserId!.Value).ToList();
            }
            return new List<Guid>();
        }
    }
}
