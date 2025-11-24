using Legacy.Domain.Entities;
using MediatR;

namespace Legacy.Contract.Commands
{
    public class FetchProductCategoryCommand : IRequest<IEnumerable<ProductCategory>>
    {
    }
}
