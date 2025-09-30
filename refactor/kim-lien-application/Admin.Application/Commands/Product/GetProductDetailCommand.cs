using Admin.Application.Models.Product;
using MediatR;

namespace Admin.Application.Commands.Product
{
    public class GetProductDetailCommand : GetDetailProductRequest, IRequest<ProductDto>
    {
    }
}
