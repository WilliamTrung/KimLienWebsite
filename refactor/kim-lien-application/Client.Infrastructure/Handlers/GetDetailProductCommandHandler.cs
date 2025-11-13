using Client.Application.Abstractions;
using Client.Application.Commands.Product;
using Client.Application.Models.Product;
using MediatR;

namespace Client.Infrastructure.Handlers
{
    public class GetDetailProductCommandHandler(IProductService productService) : IRequestHandler<GetDetailProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(GetDetailProductCommand request, CancellationToken cancellationToken)
        {
            return await productService.GetDetail(request, cancellationToken);
        }
    }
}
