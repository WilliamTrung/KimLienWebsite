using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using Admin.Application.Models.Product;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Admin.Infrastructure.Handlers.Product
{
    public class GetProductPaginationCommandHandler(IProductService productService) : IRequestHandler<GetProductPaginationCommand, PaginationResponse<ProductDto>>
    {
        public async Task<PaginationResponse<ProductDto>> Handle(GetProductPaginationCommand request, CancellationToken cancellationToken)
            => await productService.GetPaginationResponse(request, cancellationToken);
    }
}
