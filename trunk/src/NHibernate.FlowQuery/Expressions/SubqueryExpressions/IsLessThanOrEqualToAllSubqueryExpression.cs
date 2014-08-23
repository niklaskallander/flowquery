namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) less than or equal to all" filter.
    /// </summary>
    public class IsLessThanOrEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanOrEqualToAllSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsLessThanOrEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyLeAll)
        {
        }
    }
}