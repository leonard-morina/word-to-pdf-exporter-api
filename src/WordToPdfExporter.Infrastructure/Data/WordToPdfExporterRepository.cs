using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WordToPdfExporter.Core.Common;

namespace WordToPdfExporter.Infrastructure.Data;

public sealed class WordToPdfExporterRepository<T>(WordToPdfExporterContext dbContext)
    : IAsyncRepository<T> where T : class, IEntity
{
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        _currentTransaction ??= await dbContext.Database.BeginTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_currentTransaction != null) await _currentTransaction.RollbackAsync();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            if (_currentTransaction != null) await _currentTransaction.CommitAsync();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        var evaluator = new SelectorOptionalSpecificationEvaluator();
        return evaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        return await specificationResult.ToListAsync(cancellationToken);
    }

    public Task<bool> UpdateRangeAsync(IReadOnlyList<T> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator.Default.GetQuery(dbContext.Set<T>().AsQueryable(), spec);
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(specification);
        return await specificationResult.ToListAsync(cancellationToken);
    }

    public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] updatedProperties)
    {
        if (updatedProperties.Any())
        {
            dbContext.Set<T>().Attach(entity);

            foreach (var property in updatedProperties)
            {
                dbContext.Entry(entity).Property(property).IsModified = true;
            }
        }
        else
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<TResult> FirstAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(specification);
        return await specificationResult.FirstAsync(cancellationToken);
    }

    public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        return await specificationResult.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(specification);
        return await specificationResult.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T> LastAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        return await specificationResult.LastAsync(cancellationToken);
    }

    public async Task<T> LastOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        return await specificationResult.LastOrDefaultAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}