using Client.Application.Models.Product;
using MediatR;

namespace Client.Application.Commands.Product
{
    /// <summary>
    /// Command to retrieve the top 6 most viewed products based on ProductView.ViewCount
    /// </summary>
    public class GetMostViewedProductsCommand : IRequest<List<ProductDto>>
    {
        /// <summary>
        /// Maximum number of products to return (default: 6)
        /// </summary>
        public int Limit { get; set; } = 6;
    }
}
