using Chat.Application.Chat.Commands.SendMessage;
using Common.Api;
using Common.Kernel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Default)]
    [Route("/api/management/product")]
    public class ChatController(ISender sender) : ControllerBase
    {
        public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return this.CreateOk();
        }
    }
}
