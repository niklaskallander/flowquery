namespace NHibernate.FlowQuery.Expressions
{
    using System;

    using NHibernate.Criterion;

    /// <summary>
    ///     Provides the base structure of a "simple" query filter.
    /// </summary>
    public abstract class SimpleIsExpression : IsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleIsExpression"/> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        /// <param name="filterFactory">
        ///     The filter factory.
        /// </param>
        protected SimpleIsExpression(object value, Func<IProjection, object, ICriterion> filterFactory)
        {
            FilterFactory = filterFactory;
            Value = value;
        }

        /// <summary>
        ///     Gets the filter factory.
        /// </summary>
        /// <value>
        ///     The filter factory.
        /// </value>
        protected Func<IProjection, object, ICriterion> FilterFactory { get; private set; }

        /// <summary>
        ///     Gets the value used by the filter.
        /// </summary>
        /// <value>
        ///     The value used by the filter.
        /// </value>
        protected object Value { get; private set; }

        /// <summary>
        ///     Compiles this <see cref="IsExpression" /> into a <see cref="ICriterion" /> instance.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <returns>
        ///     The compiled <see cref="ICriterion" /> instance.
        /// </returns>
        protected override ICriterion CompileCore(string property)
        {
            return FilterFactory(Projections.Property(property), Value);
        }
    }
}