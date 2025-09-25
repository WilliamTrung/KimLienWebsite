namespace Common.Kernel.Models.Abstractions
{
    public interface IAuditEntity
    {
        DateTime CreatedDate { set; get; }
        DateTime ModifiedDate { set; get; }
    }
}
