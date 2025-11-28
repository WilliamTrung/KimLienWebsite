using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using MediatR;

namespace Admin.Application.Handlers.Product
{
    public class CreateProductCommandHandler(IProductService productService) : IRequestHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await productService.CreateProduct(request, cancellationToken);
        }
    }
}
