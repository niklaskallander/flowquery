namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) less than some" filter.
    /// </summary>
    public class IsLessThanSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanSomeSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsLessThanSomeSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyLtSome)
        {
        }
    }
}