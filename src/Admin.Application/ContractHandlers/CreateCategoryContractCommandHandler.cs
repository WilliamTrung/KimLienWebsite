using Admin.Application.Abstractions;
using Admin.Contract.Commands;
using MediatR;

namespace Admin.Application.ContractHandlers
{
    public class CreateCategoryContractCommandHandler(IImportDataService service) : IRequestHandler<CreateCategoryContractCommand>
    {
        public async Task Handle(CreateCategoryContractCommand request, CancellationToken cancellationToken)
        {
            await service.ImportCategory(request, cancellationToken);
        }
    }
}
