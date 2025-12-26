using Client.Application.Models.Category;
using MediatR;

namespace Client.Application.Commands.Category
{
    public class GetCategoryTreeCommand : IRequest<List<CategoryTreeDto>>
    {
    }
}
