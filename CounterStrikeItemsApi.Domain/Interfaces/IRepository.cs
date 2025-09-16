using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

        Task AddAsync(T entity);
        void Update(T entity);
        Task<bool> DeleteAsync(Guid id);
        void Delete(T entity);

        Task SaveChangesAsync();
    }
}
