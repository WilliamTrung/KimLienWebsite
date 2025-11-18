using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Kernel.Models.Abstractions;

namespace Common.Kernel.Models.Implementations
{
    public class BaseEntity<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { set; get; }
    }
    public class BaseSlugEntity<TKey> : BaseEntity<TKey>, IQueryEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Slug { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string BareName { get; set; } = null!;
        [Required]
        public string SlugName { get; set; } = null!;
    }
}
