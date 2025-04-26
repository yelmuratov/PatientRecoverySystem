using System.Linq.Expressions;

namespace PatientRecoverySystem.Application.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string orderByProperty, bool desc)
        {
            if (string.IsNullOrEmpty(orderByProperty))
                return source;

            var entityType = typeof(T);
            var property = entityType.GetProperty(orderByProperty);
            if (property == null)
                return source; // Property not found, return original source

            var parameter = Expression.Parameter(entityType, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            string method = desc ? "OrderByDescending" : "OrderBy";

            var resultExp = Expression.Call(typeof(Queryable), method,
                new Type[] { entityType, property.PropertyType },
                source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
