using Admin.Application.Abstractions;
using Admin.Application.Commands.Product;
using Admin.Contract.Commands;
using MediatR;

namespace Admin.Application.ContractHandlers
{
    public class InsertProductCategoryContractCommandHandler(IProductService productService) : IRequestHandler<InsertProductCategoryContractCommand>
    {
        public async Task Handle(InsertProductCategoryContractCommand request, CancellationToken cancellationToken)
        {
            var product = await productService.GetDetail(new Models.Product.GetDetailProductRequest
            {
                Value = request.ProductId.ToString()
            }, cancellationToken);
            var modifyProduct = new ModifyProductCommand
            {
                Id = request.ProductId.ToString(),
                Name = product.Name,
                Description = product.Description,
                CategoryIds = request.CategoryIds,
                Images = product.Images,
            };
            await productService.ModifyProduct(modifyProduct, cancellationToken);
        }
    }
}
