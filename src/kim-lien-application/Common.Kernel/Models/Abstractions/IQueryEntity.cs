namespace Common.Kernel.Models.Abstractions
{
    public interface IQueryEntity
    {
        public string Name { get; set; }
        public string BareName { get; set; }
        public string SlugName { get; set; }
    }
}
