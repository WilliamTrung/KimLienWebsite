using Client.Application.Abstractions;
using Client.Application.Commands.Category;
using Client.Application.Models.Category;
using MediatR;

namespace Client.Application.Handlers
{
    public class GetCategoryTreeCommandHandler(ICategoryService categoryService)
        : IRequestHandler<GetCategoryTreeCommand, List<CategoryTreeDto>>
    {
        public async Task<List<CategoryTreeDto>> Handle(GetCategoryTreeCommand request, CancellationToken cancellationToken)
        {
            return await categoryService.GetTree(cancellationToken);
        }
    }
}
