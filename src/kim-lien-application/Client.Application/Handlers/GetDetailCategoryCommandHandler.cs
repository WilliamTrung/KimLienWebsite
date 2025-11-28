using Client.Application.Abstractions;
using Client.Application.Commands.Category;
using Client.Application.Models.Category;
using MediatR;

namespace Client.Application.Handlers
{
    public class GetDetailCategoryCommandHandler(ICategoryService categoryService)
        : IRequestHandler<GetDetailCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetDetailCategoryCommand request, CancellationToken cancellationToken)
        {
            return await categoryService.GetDetail(request, cancellationToken);
        }
    }
}
