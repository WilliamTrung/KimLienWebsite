using Client.Application.Models.Product;
using MediatR;

namespace Client.Application.Commands.Product
{
    public class GetDetailProductCommand : GetDetailProductRequest, IRequest<ProductDto>
    {
    }
}
