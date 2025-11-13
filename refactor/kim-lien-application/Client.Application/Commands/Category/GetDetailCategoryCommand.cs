using Client.Application.Models.Category;
using MediatR;

namespace Client.Application.Commands.Category
{
    public class GetDetailCategoryCommand : GetDetailCategoryRequest, IRequest<CategoryDto>
    {
    }
}
