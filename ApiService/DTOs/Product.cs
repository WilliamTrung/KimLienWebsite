using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ApiService.DTOs
{
    public partial class Product
        {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product Name cannot be empty!")]
        [MinLength(3, ErrorMessage = "Product name is too short!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Product Description cannot be empty!")]
        //[MinLength(5, ErrorMessage = "Product Description is too short!")]
        public string Description { get; set; } = null!;
        public string? Pictures { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual IList<ProductCategory> ProductCategories { get; set; } = null!;
        [NotMapped]
        public IList<string>? DeserializedPictures { get; set; }

        
    }
}
