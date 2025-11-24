namespace Legacy.Domain.Common
{
    public class EntityBase
    {
        public Guid Id { set; get; }
    }

    public interface IAuditEntity
    {
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
    }

    public interface IDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
