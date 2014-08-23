namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) equal to all" filter.
    /// </summary>
    public class IsEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsEqualToAllSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyEqAll)
        {
        }
    }
}