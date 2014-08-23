namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) less than or equal to some" filter.
    /// </summary>
    public class IsLessThanOrEqualToSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanOrEqualToSomeSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsLessThanOrEqualToSomeSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyLeSome)
        {
        }
    }
}