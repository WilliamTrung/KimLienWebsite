using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using MediatR;

namespace Admin.Infrastructure.Handlers.Product
{
    public class DeleteProductCommandHandler(IProductService ProductService) : IRequestHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            => await ProductService.Delete(request.Id, cancellationToken);
    }
}
