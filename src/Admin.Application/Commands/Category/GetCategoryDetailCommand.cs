using Admin.Application.Models.Category;
using MediatR;

namespace Admin.Application.Commands.Category
{
    public class GetCategoryDetailCommand : GetDetailCategoryRequest, IRequest<CategoryDto>
    {
    }
}
