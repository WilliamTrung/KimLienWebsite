using Import.Application.Commands.Legacy;
using MediatR;

namespace Import.Application.Handlers.Legacy
{
    public class ImportLegacyCategoryCommandHandler : IRequestHandler<ImportLegacyCategoryCommand>
    {
        public Task Handle(ImportLegacyCategoryCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
