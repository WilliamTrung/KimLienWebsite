using Admin.Application.Models.Product;
using Common.Kernel.Request.Pagination;
using Common.Kernel.Response.Pagination;
using MediatR;

namespace Admin.Application.Commands.Product
{
    public class GetProductPaginationCommand : PaginationRequest<ProductFilterModel>, IRequest<PaginationResponse<List<ProductDto>>>
    {
    }
}
