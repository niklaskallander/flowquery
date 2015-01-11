namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) greater than" filter.
    /// </summary>
    public class IsGreaterThanSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsGreaterThanSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsGreaterThanSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyGt)
        {
        }
    }
}