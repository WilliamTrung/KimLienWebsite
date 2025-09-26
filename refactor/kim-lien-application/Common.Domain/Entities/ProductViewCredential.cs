using Common.Domain.Authen.Entities;
using Common.Kernel.Parameters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ProductViewCredential
    {
        public ProductViewType ViewType { get; set; }
        public Guid ProductViewId { get; set; }
        public Guid? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        [ForeignKey(nameof(ProductViewId))]
        public ProductView ProductView { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
