using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Infrastructure.Helpers
{
    public static class LambdaExpression
    {
        public static Expression<Func<T, bool>> GetSlugLambda<T>(string slug)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "Slug");
            var constant = Expression.Constant(slug);
            var equality = Expression.Equal(property, constant);
            return Expression.Lambda<Func<T, bool>>(equality, parameter);
        }
        public static Expression<Func<T, bool>> GetIdLambda<T>(Guid id)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "Id");
            var constant = Expression.Constant(id);
            var equality = Expression.Equal(property, constant);
            return Expression.Lambda<Func<T, bool>>(equality, parameter);
        }
    }
}
