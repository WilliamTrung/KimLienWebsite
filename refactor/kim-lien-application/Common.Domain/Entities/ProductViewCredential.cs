using Common.Kernel.Parameters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ProductViewCredential
    {
        public ProductViewType ViewType { get; set; }
        public Guid ProductViewId { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        [ForeignKey(nameof(ProductViewId))]
        public ProductView ProductView { get; set; } = null!;
    }
}
