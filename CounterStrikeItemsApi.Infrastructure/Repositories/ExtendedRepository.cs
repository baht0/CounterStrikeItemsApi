using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Infrastructure.Repositories
{
    public class ExtendedRepository<T>(AppDbContext context) : Repository<T>(context), IExtendedRepository<T> where T : class
    {
        //All
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            query = query.IncludeMany(includes);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes)
        {
            var query = _dbSet.AsQueryable();
            query = query.IncludeMany(includes, thenIncludes);
            return await query.ToListAsync();
        }

        //Find
        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate, 
            Expression<Func<T, object>>[] includes, 
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes)
        {
            var query = _dbSet.AsQueryable();
            query = query.IncludeMany(includes, thenIncludes);

            return await query.Where(predicate).ToListAsync();
        }

        //ById
        public async Task<T?> GetByIdAsync(
            Guid id,
            Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            query = query.IncludeMany(includes);
            var lambda = Helpers.LambdaExpression.GetIdLambda<T>(id);

            return await query.FirstOrDefaultAsync(lambda);
        }
        public async Task<T?> GetByIdAsync(
            Guid id,
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes)
        {
            var query = _dbSet.AsQueryable();
            query = query.IncludeMany(includes, thenIncludes);
            var lambda = Helpers.LambdaExpression.GetIdLambda<T>(id);

            return await query.FirstOrDefaultAsync(lambda);
        }

        //BySlug
        public async Task<T?> GetBySlugAsync(string slug)
        {
            var lambda = Helpers.LambdaExpression.GetSlugLambda<T>(slug);
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(lambda);
        }
        public async Task<T?> GetBySlugAsync(string slug,
            Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            query = query.IncludeMany(includes);
            var lambda = Helpers.LambdaExpression.GetSlugLambda<T>(slug);

            return await query.FirstOrDefaultAsync(lambda);
        }
        public async Task<T?> GetBySlugAsync(
            string slug,
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes)
        {
            var query = _dbSet.AsQueryable();
            query = query.IncludeMany(includes, thenIncludes);
            var lambda = Helpers.LambdaExpression.GetSlugLambda<T>(slug);

            return await query.FirstOrDefaultAsync(lambda);
        }
    }
}
