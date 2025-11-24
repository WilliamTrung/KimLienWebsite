using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Legacy.Domain.Common;

namespace Legacy.Domain.Entities
{
    public class Product : IDeleteEntity, IAuditEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Pictures { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual IList<ProductCategory>? ProductCategories { get; set; }
        [NotMapped]
        public IList<string>? DeserializedPictures { get; set; }
    }
}
