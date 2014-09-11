namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) less than all" filter.
    /// </summary>
    public class IsLessThanAllSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanAllSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsLessThanAllSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyLtAll)
        {
        }
    }
}