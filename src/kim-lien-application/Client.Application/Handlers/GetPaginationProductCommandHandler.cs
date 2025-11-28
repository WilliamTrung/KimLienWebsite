using Client.Application.Abstractions;

namespace Client.Application.Handlers
{
    public class GetPaginationProductCommandHandler(IProductService productService)
        : MediatR.IRequestHandler<Client.Application.Commands.Product.GetPaginationProductCommand, Common.Kernel.Response.Pagination.PaginationResponse<Client.Application.Models.Product.ProductDto>>
    {
        public async Task<Common.Kernel.Response.Pagination.PaginationResponse<Client.Application.Models.Product.ProductDto>> Handle(Client.Application.Commands.Product.GetPaginationProductCommand request, CancellationToken cancellationToken)
        {
            return await productService.GetPaginationResponse(request, cancellationToken);
        }
    }
}
