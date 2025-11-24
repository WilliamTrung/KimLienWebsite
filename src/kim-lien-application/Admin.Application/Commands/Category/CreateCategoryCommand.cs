using Admin.Application.Models.Category;
using MediatR;

namespace Admin.Application.Commands.Category
{
    public class CreateCategoryCommand : CreateCategoryDto, IRequest<Guid>
    {
    }
}
