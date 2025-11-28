using Client.Application.Models.Product;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Client.Application.Commands.Product
{
    public class GetPaginationProductCommand : PaginationRequest<ProductFilterModel>, IRequest<PaginationResponse<ProductDto>>
    {
    }
}
