using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanOrEqualToSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsGreaterThanOrEqualToSomeSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGeSome(property, Query.Criteria);
        }
    }
}