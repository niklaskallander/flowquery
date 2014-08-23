namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) equal to" filter.
    /// </summary>
    public class IsEqualToSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsEqualToSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsEqualToSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyEq)
        {
        }
    }
}