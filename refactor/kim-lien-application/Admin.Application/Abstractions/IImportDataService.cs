using Admin.Contract.Commands;

namespace Admin.Application.Abstractions
{
    public interface IImportDataService
    {
        Task ImportCategory(CreateCategoryContractCommand command, CancellationToken ct);
        Task ImportProduct(CreateProductContractCommand command, CancellationToken ct);
        Task ImportProductCategories(InsertProductCategoryContractCommand command, CancellationToken ct);
    }
}
