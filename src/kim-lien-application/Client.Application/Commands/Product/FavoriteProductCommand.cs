using MediatR;

namespace Client.Application.Commands.Product
{
    public class FavoriteProductCommand : IRequest
    {
        public string ProductId { get; set; } = null!;
    }
}
