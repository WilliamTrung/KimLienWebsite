using Client.Application.Abstractions;
using Client.Application.Commands.Product;
using Client.Application.Models.Product;
using MediatR;

namespace Client.Application.Handlers
{
    /// <summary>
    /// Handler for retrieving most viewed products
    /// </summary>
    public class GetMostViewedProductsCommandHandler : IRequestHandler<GetMostViewedProductsCommand, List<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetMostViewedProductsCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<ProductDto>> Handle(GetMostViewedProductsCommand request, CancellationToken cancellationToken)
        {
            return await _productService.GetMostViewedProducts(request.Limit, cancellationToken);
        }
    }
}
