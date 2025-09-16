using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Application.Helpers
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() => x => true;

        public static Expression<Func<T, bool>> False<T>() => x => false;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = Expression.AndAlso(
                Expression.Invoke(expr1, param),
                Expression.Invoke(expr2, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = Expression.OrElse(
                Expression.Invoke(expr1, param),
                Expression.Invoke(expr2, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
