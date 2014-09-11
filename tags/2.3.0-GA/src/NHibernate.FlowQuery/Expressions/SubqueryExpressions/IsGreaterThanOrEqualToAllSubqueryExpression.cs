namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) greater than or equal to all" filter.
    /// </summary>
    public class IsGreaterThanOrEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsGreaterThanOrEqualToAllSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsGreaterThanOrEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyGeAll)
        {
        }
    }
}