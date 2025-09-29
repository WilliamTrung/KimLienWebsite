namespace Admin.Application.Models.Category
{
    public class ModifyCategoryDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
