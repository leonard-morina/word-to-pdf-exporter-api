using System.Linq.Expressions;
using Ardalis.Specification;

namespace WordToPdfExporter.Core.Common;

public interface IAsyncRepository<T> where T : class, IEntity
{
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default);
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] updatedProperties);
    Task<TResult> FirstAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);
    Task<T> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<T> LastAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<T> LastOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateRangeAsync(IReadOnlyList<T> entities, CancellationToken cancellationToken = default);
    IQueryable<T> ApplySpecification(ISpecification<T> spec);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}