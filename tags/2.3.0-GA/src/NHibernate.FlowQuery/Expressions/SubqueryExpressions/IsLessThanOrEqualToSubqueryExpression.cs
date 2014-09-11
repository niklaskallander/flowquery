namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) less than or equal to" filter.
    /// </summary>
    public class IsLessThanOrEqualToSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanOrEqualToSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsLessThanOrEqualToSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyLe)
        {
        }
    }
}