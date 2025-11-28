using Client.Application.Abstractions;
using Client.Application.Commands.Product;
using Common.Domain.Entities;
using Common.DomainException.Abstractions;
using Common.RequestContext.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Client.Application.Handlers
{
    public class FavoriteProductCommandHandler(IClientDbContext dbContext, IRequestContext requestContext) : IRequestHandler<FavoriteProductCommand>
    {
        public async Task Handle(FavoriteProductCommand request, CancellationToken cancellationToken)
        {
             if (Guid.TryParse(request.ProductId, out var productId))
            {
                var product = await dbContext.Products.AsQueryable().Where(x => x.Id == productId)
                                                .Include(x => x.ProductFavors).FirstOrDefaultAsync();
                if (product is null)
                {
                    throw new CNotFoundException(typeof(Product));
                }
                var userId = Guid.Parse(requestContext.Data.UserId!);
                var favor = product.ProductFavors.FirstOrDefault(x => Guid.Equals(x.UserId, userId));
                if (favor is null)
                {
                    product.ProductFavors.Add(new ProductFavor
                    {
                        ProductId = productId,
                        UserId = userId,
                        Email = requestContext.Data.Email!,
                    });
                }
                else
                {
                    product.ProductFavors.Remove(favor);
                }
                dbContext.Products.Update(product);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new CInvalidException(typeof(Product));
            }
        }
    }
}
