using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppService.DTOs
{
    public partial class Product
        {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product Name cannot be empty!")]
        [MinLength(3, ErrorMessage = "Product name is too short!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Product Description cannot be empty!")]
        [MinLength(5, ErrorMessage = "Product Description is too short!")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Category cannot be empty!")]
        public Guid CategoryId { get; set; }
        [Required(ErrorMessage = "Product must have pictures!")]
        public string? Pictures { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual IList<Category>? Categories { get; set; }
        [NotMapped]
        public IList<string>? DeserializedPictures { get; set; }
    }
}
