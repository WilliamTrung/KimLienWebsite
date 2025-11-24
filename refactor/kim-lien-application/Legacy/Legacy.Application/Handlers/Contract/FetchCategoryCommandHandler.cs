using Legacy.Contract.Commands;
using Legacy.Domain.Entities;
using Legacy.Infrastructure.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Legacy.Application.Handlers.Contract
{
    public class FetchCategoryCommandHandler(SqlContext dbContext) : IRequestHandler<FetchCategoryCommand, IEnumerable<Legacy.Domain.Entities.Category>>
    {
        public async Task<IEnumerable<Category>> Handle(FetchCategoryCommand request, CancellationToken cancellationToken)
        {
            var categories = await dbContext.Categories.ToListAsync();
            return categories;
        }
    }
}
