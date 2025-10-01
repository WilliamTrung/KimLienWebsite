namespace Client.Application.Abstractions.Models.Category
{
    public class CategoryFilterModel
    {
        public string? ParentId { get; set; }
        public List<string>? Ids { get;set; }
        public List<string>? Name { get; set; }
    }
}
