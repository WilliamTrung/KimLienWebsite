using Client.Application.Models.Product;
using MediatR;

namespace Client.Application.Commands.Product
{
    /// <summary>
    /// Command to retrieve "hot" trending products with high view count in the last 30 days
    /// </summary>
    public class GetHotProductsCommand : IRequest<List<ProductDto>>
    {
        /// <summary>
        /// Maximum number of products to return (default: 6)
        /// </summary>
        public int Limit { get; set; } = 6;
    }
}
