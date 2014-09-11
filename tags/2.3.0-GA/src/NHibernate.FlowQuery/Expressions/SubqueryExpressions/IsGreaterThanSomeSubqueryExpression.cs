namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) greater than some" filter.
    /// </summary>
    public class IsGreaterThanSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsGreaterThanSomeSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsGreaterThanSomeSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyGtSome)
        {
        }
    }
}