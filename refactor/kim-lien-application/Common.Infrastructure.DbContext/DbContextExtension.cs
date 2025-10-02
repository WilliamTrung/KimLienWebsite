using Microsoft.EntityFrameworkCore;
using ContextCore = Microsoft.EntityFrameworkCore.DbContext;
namespace Common.Infrastructure.DbContext
{
    public static class DbContextExtension
    {
        // 1) No parameter → returns TResponse
        public static Task<TResponse> ExecuteTransactionAsync<TResponse>(
            this ContextCore db,
            Func<CancellationToken, Task<TResponse>> work,
            CancellationToken ct = default)
            => ExecuteCoreAsync(db, ct, work);

        // 2) With parameter → returns TResponse
        public static Task<TResponse> ExecuteTransactionAsync<TState, TResponse>(
            this ContextCore db,
            TState state,
            Func<TState, CancellationToken, Task<TResponse>> work,
            CancellationToken ct = default)
            => ExecuteCoreAsync(db, ct, tok => work(state, tok));

        // 3) No parameter → no response
        public static Task ExecuteTransactionAsync(
            this ContextCore db,
            Func<CancellationToken, Task> work,
            CancellationToken ct = default)
            => ExecuteCoreAsync(db, ct, async tok => { await work(tok); return true; });

        // 4) With parameter → no response
        public static Task ExecuteTransactionAsync<TState>(
            this ContextCore db,
            TState state,
            Func<TState, CancellationToken, Task> work,
            CancellationToken ct = default)
            => ExecuteCoreAsync(db, ct, async tok => { await work(state, tok); return true; });

        // -------- core with resilient execution + proper disposal ----------
        private static async Task<T> ExecuteCoreAsync<T>(
            ContextCore db,
            CancellationToken ct,
            Func<CancellationToken, Task<T>> body)
        {
            var strategy = db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                try
                {
                    var result = await body(ct);
                    await tx.CommitAsync(ct);
                    return result;
                }
                catch
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
            });
        }
    }
}
