using Admin.Application.Abstractions;
using Admin.Application.Commands.Category;
using MediatR;

namespace Admin.Application.Handlers.Category
{
    public class CreateCategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<CreateCategoryCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            => await categoryService.Create(request, cancellationToken);
    }
}
