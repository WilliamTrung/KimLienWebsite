namespace Client.Application.Models.Product
{
    public class ProductCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
