namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) greater than or equal to some" filter.
    /// </summary>
    public class IsGreaterThanOrEqualToSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsGreaterThanOrEqualToSomeSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsGreaterThanOrEqualToSomeSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyGeSome)
        {
        }
    }
}