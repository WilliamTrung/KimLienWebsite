using Common.Kernel.Models.Abstractions;
using Common.Kernel.Models.Implementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.Entities
{
    /// <summary>
    /// Pricing record for products - supports fixed price, range, and contact-for-price
    /// Only one pricing record can be active per product at a time
    /// </summary>
    public class PricingRecord : BaseEntity<Guid>, IAuditEntity, IDeleteEntity
    {

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Type of pricing: "fixed", "range", "contact"
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = null!;

        /// <summary>
        /// Fixed price value (used when Type = "fixed")
        /// </summary>
        public decimal? FixedPrice { get; set; }

        /// <summary>
        /// Minimum price (used when Type = "range")
        /// </summary>
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Maximum price (used when Type = "range")
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Only one pricing record per product can be active
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public User Creator { get; set; } = null!;

        [ForeignKey(nameof(ModifiedBy))]
        public User? Modifier { get; set; }

        public bool IsDeleted { get; set; }
    }
}
