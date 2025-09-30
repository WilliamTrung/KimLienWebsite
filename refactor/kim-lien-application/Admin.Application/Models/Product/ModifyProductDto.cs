namespace Admin.Application.Models.Product
{
    public class ModifyProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Guid> CategoryIds { get; set; } = null!; // List of associated category IDs
    }
}
