using Import.Domain.Events;
using MediatR;

namespace Import.Application.Commands.Import.Handlers
{
    public class ImportDataEventHandler(ISender sender) : IRequestHandler<ImportDataEvent>
    {
        public async Task Handle(ImportDataEvent request, CancellationToken cancellationToken)
        {
            var importLegacyCategoryCommand = new Legacy.ImportLegacyCategoryCommand();
            await sender.Send(importLegacyCategoryCommand, cancellationToken);
            var importLegacyProductCommand = new Legacy.ImportLegacyProductCommand();
            await sender.Send(importLegacyProductCommand, cancellationToken);
            var importLegacyProductCategoryCommand = new Legacy.ImportLegacyProductCategoryCommand();
            await sender.Send(importLegacyProductCategoryCommand, cancellationToken);
        }
    }
}
