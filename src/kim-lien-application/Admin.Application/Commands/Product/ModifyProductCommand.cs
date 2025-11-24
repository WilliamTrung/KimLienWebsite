using Admin.Application.Models.Product;
using MediatR;

namespace Admin.Application.Commands.Product
{
    public class ModifyProductCommand : ModifyProductDto, IRequest
    {
    }
}
