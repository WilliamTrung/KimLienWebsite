using AutoMapper;
using Chat.Application.Chat.Abstractions;
using Chat.Application.Chat.Models;
using Chat.Application.Common.Abstractions;
using Chat.Application.Common.Models;
using Chat.Infrastructure.Data;
using Chat.Infrastructure.DataHub;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.Extension;
using Common.RequestContext.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Common.Extension.Logging;
namespace Chat.Infrastructure.Services
{
    public class ChatService(IRequestContext requestContext
        , ChatContext chatContext
        , IMapper mapper
        , IHubContext<ChatHub> chatHub
        , IConnectionPoolProvider connectionPoolProvider
        , ILogger<ChatService> logger)
        : IChatService
    {
        public async Task SendMessage(MessageDto messageDto)
        {
            var userId = Guid.Parse(requestContext.Data.UserId!);
            var user = await chatContext.Users.Where(x => x.Id == userId).Include(x => x.UserMetadata).FirstOrDefaultAsync();
            if (user is null)
            {
                logger.LogDataInformation($"User not found for id: {userId}");
                throw new CException("User not found", System.Net.HttpStatusCode.NotFound);
            }
            // Here you would implement the logic to send the message,
            // for example, saving it to a database or sending it through a messaging system.
            var message = new ChatMessage
            {
                RoomId = Guid.Parse(messageDto.RoomId),
                SenderId = user.Id,
                Message = messageDto.Message,
                Metadata = messageDto.Metadata?.ToDocument(),
                IpAddress = requestContext.Data.IpAddress ?? throw new CException("IpAddress is null", System.Net.HttpStatusCode.BadRequest)
            };
            chatContext.Add(message);
            await chatContext.SaveChangesAsync();
            var userDto = mapper.Map<UserDto>(user);
            await chatHub.Clients.GroupExcept(messageDto.RoomId, connectionPoolProvider.GetConnection(userId.ToString()))
                                 .SendAsync("ReceiveMessage", new
                                 {
                                     Sender = userDto,
                                     Message = messageDto
                                 });
        }
        public async Task<List<Guid>> GetRoomIdsByUser(string userId)
        {
            if (Guid.TryParse(userId, out var userGuid))
            {
                var sessions = await chatContext.ChatSessions.Where(x => x.UserId.HasValue && x.UserId == userGuid).ToListAsync();
                return sessions.Select(x => x.RoomId).ToList();
            }
            return new List<Guid>();
        }
    }
}
