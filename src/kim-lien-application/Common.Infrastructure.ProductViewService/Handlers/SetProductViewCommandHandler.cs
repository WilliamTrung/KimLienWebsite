using Common.Application.ProductViewService.Abstractions;
using Common.Application.ProductViewService.Commands;
using Common.Domain.Entities;
using Common.RequestContext.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.ProductViewService.Handlers
{
    public class SetProductViewCommandHandler(IRequestContext requestContext) : IRequestHandler<SetProductViewCommand>
    {
        public async Task Handle(SetProductViewCommand request, CancellationToken cancellationToken)
        {
            var dbContext = (IProductViewDbContext)request.DbContext;
            var productView = await dbContext.ProductViews.Where(x => x.ProductId == request.ProductId)
                                                            .Include(x => x.ProductViewCredentials.Where(x => x.IpAddress == requestContext.Data.IpAddress!))
                                                            .FirstOrDefaultAsync();
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (productView is not null)
            {
                if (user is null && !productView.ProductViewCredentials.Any())
                {
                    //handle anonymous view count
                    productView.ProductViewCredentials.Add(new ProductViewCredential
                    {
                        UserId = null,
                        ViewCount = 1,
                        Email = null,
                        Username = null,
                        ViewType = Kernel.Parameters.ProductViewType.Anonymous,
                        IpAddress = requestContext.Data.IpAddress!,
                    });
                    productView.ViewCount += 1;
                }
                else
                {
                    //handle authorized view count
                    var credential = productView.ProductViewCredentials
                                        .FirstOrDefault(x => x.UserId == request.UserId);
                    if (credential is not null)
                    {
                        if (credential.CreatedDate.Date != DateTime.UtcNow.Date && (credential.ModifiedDate is null || credential.ModifiedDate?.Date < DateTime.UtcNow.Date))
                        {
                            credential.ViewCount += 1;
                            productView.ViewCount += 1;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        productView.ProductViewCredentials.Add(GenerateCredentialData(user));
                    }
                }
                dbContext.ProductViews.Update(productView);
            }
            else
            {
                //init product view
                productView = new ProductView
                {
                    ProductId = request.ProductId,
                    ViewCount = 1,
                    ProductViewCredentials = new List<ProductViewCredential>
                    {
                        GenerateCredentialData(user)
                    }
                };
                dbContext.ProductViews.Add(productView);
            }
            await request.DbContext.SaveChangesAsync();
        }
        private ProductViewCredential GenerateCredentialData(User? user)
        {
            return new ProductViewCredential
            {
                UserId = user?.Id,
                ViewCount = 1,
                Email = user?.Email,
                Username = user?.UserName,
                ViewType = user is null ? Kernel.Parameters.ProductViewType.Anonymous : Kernel.Parameters.ProductViewType.Authorized,
                IpAddress = requestContext.Data.IpAddress!,
            };
        }
    }
}
