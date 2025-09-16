using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Infrastructure.Helpers
{
    public static class QueryableIncludeExtensions
    {
        public static IQueryable<T> IncludeMany<T>(
            this IQueryable<T> query,
            params Expression<Func<T, object>>[] includes)
            where T : class
        {
            foreach (var include in includes)
                query = query.Include(include);
            return query;
        }

        public static IQueryable<T> IncludeMany<T>(
            this IQueryable<T> query,
            Expression<Func<T, object>>[] includes,
            (Expression<Func<T, object>> include, Expression<Func<object, object>> thenInclude)[] thenIncludes)
            where T : class
        {
            foreach (var include in includes)
                query = query.Include(include);

            foreach (var (include, thenInclude) in thenIncludes)
                query = query.Include(include).ThenInclude(thenInclude);

            return query;
        }
    }
}
