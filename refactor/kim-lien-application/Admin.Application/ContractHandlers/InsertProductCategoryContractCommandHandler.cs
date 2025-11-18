using Admin.Application.Abstractions;
using Admin.Contract.Commands;
using MediatR;

namespace Admin.Application.ContractHandlers
{
    public class InsertProductCategoryContractCommandHandler(IImportDataService service) : IRequestHandler<InsertProductCategoryContractCommand>
    {
        public async Task Handle(InsertProductCategoryContractCommand request, CancellationToken cancellationToken)
        {
            await service.ImportProductCategories(request, cancellationToken);
        }
    }
}
