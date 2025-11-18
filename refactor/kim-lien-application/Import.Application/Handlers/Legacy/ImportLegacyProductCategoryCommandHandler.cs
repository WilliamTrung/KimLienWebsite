using Import.Application.Commands.Legacy;
using Legacy.Contract.Commands;
using MediatR;

namespace Import.Application.Handlers.Legacy
{
    public class ImportLegacyProductCategoryCommandHandler(ISender sender) : IRequestHandler<ImportLegacyProductCategoryCommand>
    {
        public async Task Handle(ImportLegacyProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var fetchCommand = new FetchProductCategoryCommand();
            var data = await sender.Send(fetchCommand, cancellationToken);
            var groupedData = data.GroupBy(x => x.ProductId)
                                  .Select(g => new
                                  {
                                      ProductId = g.Key,
                                      CategoryIds = g.Select(x => x.CategoryId).ToList()
                                  }).ToList();
            foreach (var item in groupedData)
            {
                var adminCommand = new Admin.Contract.Commands.InsertProductCategoryContractCommand
                {
                    ProductId = item.ProductId,
                    CategoryIds = item.CategoryIds,
                };
                await sender.Send(adminCommand, cancellationToken);
            }
        }
    }
}
