namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Represents a "is (not) less than" filter.
    /// </summary>
    public class IsLessThanSubqueryExpression : SubqueryIsExpressionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanSubqueryExpression" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        public IsLessThanSubqueryExpression(IDetachedImmutableFlowQuery query)
            : base(query, Subqueries.PropertyLt)
        {
        }
    }
}