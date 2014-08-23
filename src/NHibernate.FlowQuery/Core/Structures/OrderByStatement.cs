namespace NHibernate.FlowQuery.Core.Structures
{
    using System;

    using NHibernate.Criterion;

    /// <summary>
    ///     A class representing an order by statement.
    /// </summary>
    public class OrderByStatement
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the order by statement is based on the query source entity.
        /// </summary>
        /// <value>
        ///     A value indicating whether the order by statement is based on the query source entity.
        /// </value>
        public virtual bool IsBasedOnSource { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Order" /> value.
        /// </summary>
        /// <value>
        ///     The <see cref="Order" /> value.
        /// </value>
        public virtual Order Order { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to order ascending.
        /// </summary>
        /// <value>
        ///     A value indicating whether to order ascending.
        /// </value>
        public virtual bool OrderAscending { get; set; }

        /// <summary>
        ///     Gets or sets the projection source type.
        /// </summary>
        /// <value>
        ///     The projection source type.
        /// </value>
        public virtual Type ProjectionSourceType { get; set; }

        /// <summary>
        ///     Gets or sets the ordered property.
        /// </summary>
        /// <value>
        ///     The ordered property.
        /// </value>
        public virtual string Property { get; set; }
    }
}