using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.DTOs
{
	public partial class Category
	{
		public Guid Id { get; set; }
		[Required(ErrorMessage = "Category name cannot be empty!")]
		[MinLength(1, ErrorMessage = "Category name is too short!")]
		public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }

		public Category? Parent { get; set; }
    }
}
