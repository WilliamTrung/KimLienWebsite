using MediatR;

namespace Admin.Application.Commands.Product
{
    public class UpdateProductStatusCommand : IRequest
    {
        public string Id { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
