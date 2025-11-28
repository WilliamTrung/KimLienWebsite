using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using Common.DomainException.Abstractions;
using Common.Kernel.Constants;
using Common.Kernel.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Admin.Application.Handlers.Product
{
    public class UpdateProductStatusCommandHandler(IAdminDbContext dbContext) : IRequestHandler<UpdateProductStatusCommand>
    {
        public async Task Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(request.Id, out var id))
            {
                if (string.IsNullOrWhiteSpace(request.Status))
                {
                    throw new ArgumentException("Status cannot be null or empty", nameof(request.Status));
                }
                if (request.Status.IsValueInType(typeof(ProductStatus)))
                {
                    await dbContext.Products.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, request.Status));
                }
                else
                {
                    throw new CException("Invalid status");
                }
            }
        }
    }
}
