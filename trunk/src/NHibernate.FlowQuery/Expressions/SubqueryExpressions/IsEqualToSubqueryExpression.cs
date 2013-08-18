using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsEqualToSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsEqualToSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyEq(property, Query.Criteria);
        }
    }
}