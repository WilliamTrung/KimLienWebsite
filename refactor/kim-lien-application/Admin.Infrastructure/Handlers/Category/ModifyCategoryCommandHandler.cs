using Admin.Application.Abstractions;
using Admin.Application.Commands.Category;
using MediatR;

namespace Admin.Infrastructure.Handlers.Category
{
    public class ModifyCategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<ModifyCategoryCommand>
    {
        public async Task Handle(ModifyCategoryCommand request, CancellationToken cancellationToken)
            => await categoryService.Update(request, cancellationToken);
    }
}
