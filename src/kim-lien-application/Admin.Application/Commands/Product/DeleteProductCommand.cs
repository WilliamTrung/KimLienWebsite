using MediatR;

namespace Admin.Application.Commands.Product
{
    public class DeleteProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
