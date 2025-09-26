namespace Common.Kernel.Models.Abstractions
{
    public interface IAuditEntity
    {
        DateTime CreatedDate { set; get; }
        DateTime? ModifiedDate { set; get; }
        Guid? ModifiedBy { set; get; }
        Guid CreatedBy { set; get; }
    }
}
