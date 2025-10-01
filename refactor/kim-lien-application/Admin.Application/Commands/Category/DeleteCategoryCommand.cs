using MediatR;

namespace Admin.Application.Commands.Category
{
    public class DeleteCategoryCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
