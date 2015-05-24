namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Structures;

    /// <summary>
    ///     The order helper.
    /// </summary>
    public static class OrderHelper
    {
        /// <summary>
        ///     Generates new orders based on the underlying source by resolving the appropriate projections from the 
        ///     given lambda expression.
        /// </summary>
        /// <param name="orderByStatements">
        ///     The order by statements.
        /// </param>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <param name="data">
        ///     The helper data.
        /// </param>
        /// <returns>
        ///     The new set of orders.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="orderByStatements" />, <paramref name="projection" /> or <paramref name="data" /> is 
        ///     null.
        /// </exception>
        public static IEnumerable<OrderByStatement> GetSourceBasedOrdersFrom
            (
            IEnumerable<OrderByStatement> orderByStatements,
            LambdaExpression projection,
            QueryHelperData data
            )
        {
            if (orderByStatements == null)
            {
                throw new ArgumentNullException("orderByStatements");
            }

            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (orderByStatements.All(x => x.IsBasedOnSource))
            {
                return orderByStatements;
            }

            ProjectionHelper.GetProjection(projection, new HelperContext(data, projection, HelperType.Select));

            var orders = new List<OrderByStatement>();

            foreach (var statement in orderByStatements)
            {
                if (statement.IsBasedOnSource)
                {
                    orders.Add(statement);

                    continue;
                }

                IProjection orderProjection;

                bool foundMapping = data.Mappings
                    .TryGetValue(statement.Property, out orderProjection);

                if (foundMapping)
                {
                    var order = new OrderByStatement
                    {
                        IsBasedOnSource = true,
                        Order = new Order(orderProjection, statement.OrderAscending)
                    };

                    orders.Add(order);
                }
            }

            return orders;
        }
    }
}
