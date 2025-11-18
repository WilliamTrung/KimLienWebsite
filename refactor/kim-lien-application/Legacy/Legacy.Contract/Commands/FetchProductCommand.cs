using Legacy.Domain.Entities;
using MediatR;

namespace Legacy.Contract.Commands
{
    public class FetchProductCommand : IRequest<IEnumerable<Product>>
    {
    }
}
