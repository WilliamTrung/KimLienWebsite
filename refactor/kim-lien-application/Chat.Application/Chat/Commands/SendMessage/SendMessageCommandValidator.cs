using FluentValidation;

namespace Chat.Application.Chat.Commands.SendMessage
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(x => x.RoomId)
                .NotEmpty()
                .WithMessage("RoomId is required.")
                .Must(x => Guid.TryParse(x, out var t));
        }
    }
}
