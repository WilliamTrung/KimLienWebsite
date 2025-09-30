using Admin.Application.Models.Product;
using MediatR;

namespace Admin.Application.Commands.Product
{
    public class CreateProductCommand : CreateProductDto, IRequest<Guid>
    {

    }
}
