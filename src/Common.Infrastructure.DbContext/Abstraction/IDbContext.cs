namespace Common.Infrastructure.DbContext.Abstraction
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
