using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface IExtendedRepository<T> : IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes);

        Task<T?> GetByIdAsync(
            Guid id,
            Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(
            Guid id,
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes);

        Task<T?> GetBySlugAsync(string slug);
        Task<T?> GetBySlugAsync(string slug,
            Expression<Func<T, object>>[] includes);
        Task<T?> GetBySlugAsync(
            string slug,
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes);
    }
}
