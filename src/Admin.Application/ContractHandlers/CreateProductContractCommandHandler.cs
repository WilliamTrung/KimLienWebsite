using Admin.Application.Abstractions;
using Admin.Contract.Commands;
using MediatR;

namespace Admin.Application.ContractHandlers
{
    public class CreateProductContractCommandHandler(IImportDataService service) : IRequestHandler<CreateProductContractCommand>
    {
        public async Task Handle(CreateProductContractCommand request, CancellationToken cancellationToken)
        {
            await service.ImportProduct(request, cancellationToken);
        }
    }
}
