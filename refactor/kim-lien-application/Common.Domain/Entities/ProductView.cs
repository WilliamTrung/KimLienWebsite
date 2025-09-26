using Common.Kernel.Models.Implementations;
using Common.Kernel.Parameters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ProductView : BaseEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public string IpAddress { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        public virtual ICollection<ProductViewCredential> ProductViewCredentials { get; set; } = new List<ProductViewCredential>();
    }
}
