using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using Admin.Application.Models.Product;
using MediatR;

namespace Admin.Application.Handlers.Product
{
    public class GetProductDetailCommandHandler(IProductService productService) : IRequestHandler<GetProductDetailCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductDetailCommand request, CancellationToken cancellationToken)
        {
            return await productService.GetDetail(request, cancellationToken);
        }
    }
}
