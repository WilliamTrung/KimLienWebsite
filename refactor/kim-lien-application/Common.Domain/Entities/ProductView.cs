using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ProductView : BaseEntity<Guid>
    {
        public int ViewCount { get; set; }
        public Guid ProductId { get; set; }
        public string IpAddress { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        public virtual ICollection<ProductViewCredential> ProductViewCredentials { get; set; } = new List<ProductViewCredential>();
    }
}
