using Common.Domain.Authen.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    public class ProductFavor
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
