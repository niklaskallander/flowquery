namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) in" filter.
    /// </summary>
    public class IsInSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsInSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsInSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyIn)
        {
        }
    }
}