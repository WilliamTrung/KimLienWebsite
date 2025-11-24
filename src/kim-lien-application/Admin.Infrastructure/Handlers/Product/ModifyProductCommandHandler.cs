using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using MediatR;

namespace Admin.Infrastructure.Handlers.Product
{
    public class ModifyProductCommandHandler(IProductService productService) : IRequestHandler<ModifyProductCommand>
    {
        public async Task Handle(ModifyProductCommand request, CancellationToken cancellationToken)
            => await productService.ModifyProduct(request, cancellationToken);
    }
}
