using Import.Application.Commands.Legacy;
using Legacy.Contract.Commands;
using MediatR;

namespace Import.Application.Handlers.Legacy
{
    public class ImportLegacyCategoryCommandHandler(ISender sender) : IRequestHandler<ImportLegacyCategoryCommand>
    {
        public async Task Handle(ImportLegacyCategoryCommand request, CancellationToken cancellationToken)
        {
            var fetchCommand = new FetchCategoryCommand();
            var categories = await sender.Send(fetchCommand, cancellationToken);
            foreach (var category in categories)
            {
                var adminCommand = new Admin.Contract.Commands.CreateCategoryContractCommand
                {
                    Id = category.Id,
                    Name = category.Name ?? string.Empty,
                    ParentId = category.ParentId,
                    Pictures = null,
                }; 
                await sender.Send(adminCommand, cancellationToken);
            }
        }
    }
}
