namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) greater than all" filter.
    /// </summary>
    public class IsGreaterThanAllSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsGreaterThanAllSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsGreaterThanAllSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyGtAll)
        {
        }
    }
}