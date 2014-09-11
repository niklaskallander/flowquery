namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using System;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Provides base functionality and structure for a subquery filter.
    /// </summary>
    public abstract class SubqueryIsExpressionBase : IsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SubqueryIsExpressionBase"/> class.
        /// </summary>
        /// <param name="query">
        ///     The subquery to be used by the filter.
        /// </param>
        /// <param name="filterFactory">
        ///     The filter factory.
        /// </param>
        protected SubqueryIsExpressionBase
            (
            IDetachedImmutableFlowQuery query,
            Func<string, DetachedCriteria, ICriterion> filterFactory
            )
        {
            FilterFactory = filterFactory;
            Query = query;
        }

        /// <summary>
        ///     Gets the filter factory.
        /// </summary>
        /// <value>
        ///     The filter factory.
        /// </value>
        protected Func<string, DetachedCriteria, ICriterion> FilterFactory { get; private set; }

        /// <summary>
        ///     Gets the subquery.
        /// </summary>
        /// <value>
        ///     The subquery.
        /// </value>
        protected virtual IDetachedImmutableFlowQuery Query { get; private set; }

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
            return FilterFactory(property, Query.Criteria);
        }
    }
}