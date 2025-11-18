using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using Admin.Contract.Commands;
using AutoMapper;
using MediatR;

namespace Admin.Application.ContractHandlers
{
    public class CreateProductContractCommandHandler(IMapper mapper, IProductService productService) : IRequestHandler<CreateProductContractCommand>
    {
        public async Task Handle(CreateProductContractCommand request, CancellationToken cancellationToken)
        {
            var command = mapper.Map<CreateProductCommand>(request);
            await productService.CreateProduct(command, cancellationToken);
        }
    }
}
