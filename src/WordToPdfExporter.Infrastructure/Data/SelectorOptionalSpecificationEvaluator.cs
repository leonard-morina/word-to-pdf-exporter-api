using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace WordToPdfExporter.Infrastructure.Data;

/// <summary>
/// This will inherit from SpecificationEvaluator the only difference between this and SpecificationEvaluator
/// is that this will skip the select in cases when the type of T is the same as the type of TResult
/// </summary>
public sealed class SelectorOptionalSpecificationEvaluator : SpecificationEvaluator
{
    public IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query,
        ISpecification<T, TResult> specification) where T : class
    {
        if (specification is null) throw new ArgumentNullException("Specification is required");
        var typesDiffer = typeof(T) != typeof(TResult);
        if (typesDiffer)
        {
            if (specification.Selector is null && specification.SelectorMany is null)
                throw new SelectorNotFoundException();
            if (specification.Selector != null && specification.SelectorMany != null)
                throw new ConcurrentSelectorsException();
        }

        query = GetQuery(query, (ISpecification<T>)specification);

        if (!typesDiffer) return (IQueryable<TResult>)query;
        return specification.Selector is not null
            ? query.Select(specification.Selector)
            : query.SelectMany(specification.SelectorMany!);
    }
}