using Client.Application.Abstractions;
using Client.Application.Commands.Product;
using Client.Application.Models.Product;
using MediatR;

namespace Client.Infrastructure.Handlers
{
    /// <summary>
    /// Handler for retrieving hot/trending products
    /// </summary>
    public class GetHotProductsCommandHandler : IRequestHandler<GetHotProductsCommand, List<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetHotProductsCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<ProductDto>> Handle(GetHotProductsCommand request, CancellationToken cancellationToken)
        {
            return await _productService.GetHotProducts(request.Limit, cancellationToken);
        }
    }
}
