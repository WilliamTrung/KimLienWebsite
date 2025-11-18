using Legacy.Domain.Entities;
using MediatR;

namespace Legacy.Contract.Commands
{
    public class FetchCategoryCommand : IRequest<IEnumerable<Category>>
    {
    }
}
