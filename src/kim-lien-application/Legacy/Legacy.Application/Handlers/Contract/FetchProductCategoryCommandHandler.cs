using Legacy.Contract.Commands;
using Legacy.Domain.Entities;
using Legacy.Infrastructure.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Legacy.Application.Handlers.Contract
{
    public class FetchProductCategoryCommandHandler(SqlContext dbContext) : IRequestHandler<FetchProductCategoryCommand, IEnumerable<Legacy.Domain.Entities.ProductCategory>>
    {
        public async Task<IEnumerable<ProductCategory>> Handle(FetchProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await dbContext.ProductCategories.ToListAsync();
            return result;
        }
    }
}
