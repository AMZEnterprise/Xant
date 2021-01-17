using System;
using System.Linq;
using System.Linq.Expressions;

namespace Xant.MVC.Areas.Panel.Extensions
{
    /// <summary>
    /// Linq extensions
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Query order direction
        /// </summary>
        public enum Order
        {
            /// <summary>
            /// Ascending
            /// </summary>
            Asc,
            /// <summary>
            /// Descending
            /// </summary>
            Desc
        }

        /// <summary>
        /// Order linq query dynamically
        /// </summary>
        /// <typeparam name="T">entity type</typeparam>
        /// <param name="query">linq query</param>
        /// <param name="orderMember">entity ordering member</param>
        /// <param name="direction">order direction</param>
        /// <returns>ordered linq query</returns>
        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderMember,
            Order direction
        )
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderMember);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == Order.Asc ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));


            return query.Provider.CreateQuery<T>(orderBy);
        }
    }
}
