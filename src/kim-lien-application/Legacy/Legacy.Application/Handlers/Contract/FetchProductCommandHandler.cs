using Legacy.Contract.Commands;
using Legacy.Domain.Entities;
using Legacy.Infrastructure.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Legacy.Application.Handlers.Contract
{
    public class FetchProductCommandHandler(SqlContext dbContext) : IRequestHandler<FetchProductCommand, IEnumerable<Legacy.Domain.Entities.Product>>
    {
        public async Task<IEnumerable<Product>> Handle(FetchProductCommand request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Products.ToListAsync();
            return result;
        }
    }
}
