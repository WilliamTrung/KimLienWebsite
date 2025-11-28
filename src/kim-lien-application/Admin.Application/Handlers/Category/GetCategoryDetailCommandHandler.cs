using Admin.Application.Abstractions;
using Admin.Application.Commands.Category;
using Admin.Application.Models.Category;
using MediatR;

namespace Admin.Application.Handlers.Category
{
    public class GetCategoryDetailCommandHandler(ICategoryService categoryService) : IRequestHandler<GetCategoryDetailCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryDetailCommand request, CancellationToken cancellationToken)
            => await categoryService.GetDetail(request, cancellationToken);
    }
}
