using Admin.Application.Abstractions;
using Admin.Application.Commands.Category;
using MediatR;

namespace Admin.Application.Handlers.Category
{
    public class DeleteCategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<DeleteCategoryCommand>
    {
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            => await categoryService.Delete(request.Id, cancellationToken);
    }
}
