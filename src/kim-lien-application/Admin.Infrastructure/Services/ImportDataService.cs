using Admin.Application.Abstractions;
using Admin.Contract.Commands;
using Admin.Infrastructure.Data;
using AutoMapper;
using Common.Domain.Entities;
using Common.Kernel.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Services
{
    public class ImportDataService(IMapper mapper, AdminDbContext dbContext) : IImportDataService, IScoped
    {
        public async Task ImportCategory(CreateCategoryContractCommand command, CancellationToken ct)
        {
            var category = mapper.Map<Common.Domain.Entities.Category>(command);
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync(ct);
        }

        public async Task ImportProduct(CreateProductContractCommand command, CancellationToken ct)
        {
            var data = mapper.Map<Common.Domain.Entities.Product>(command);
            dbContext.Products.Add(data);
            await dbContext.SaveChangesAsync(ct);
        }

        public async Task ImportProductCategories(InsertProductCategoryContractCommand command, CancellationToken ct)
        {
            var category = await dbContext.Categories.Where(x => command.CategoryIds.Contains(x.Id))
                                                     .Include(x => x.Parent)
                                                     .ToListAsync();
            var requestAddCategories = category.SelectMany(x => x.Families().Select(c => c.Id)).Distinct();
            var productCategories = requestAddCategories.Select(categoryId => new Common.Domain.Entities.ProductCategory
            {
                ProductId = command.ProductId,
                CategoryId = categoryId,
                IsDeleted = false,
            });
            dbContext.ProductCategories.AddRange(productCategories);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}
