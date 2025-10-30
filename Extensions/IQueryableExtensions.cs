using System.Linq.Expressions;
using System.Reflection;

namespace inventory_management_system.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string? sortColumn)
        {
            return ApplyOrdering(source, sortColumn, ascending: true);
        }

        public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string? sortColumn)
        {
            return ApplyOrdering(source, sortColumn, ascending: false);
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> source, string? sortColumn, bool ascending)
        {
            // load all available properties of the entity type
            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // check column validity
            string selectedColumn = props.Any(p => p.Name.Equals(sortColumn, StringComparison.OrdinalIgnoreCase))
                ? props.First(p => p.Name.Equals(sortColumn, StringComparison.OrdinalIgnoreCase)).Name
                : props.First().Name; // fallback to first property (usually the PK)

            // Build expresionn
            var parameter = Expression.Parameter(type, "x");
            Expression property = parameter;

            foreach (var member in selectedColumn.Split('.'))
                property = Expression.PropertyOrField(property, member);

            var lambda = Expression.Lambda(property, parameter);
            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            // Call Queryable.OrderBy / OrderByDescending
            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { type, property.Type },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<T>(result);
        }
    }
}
