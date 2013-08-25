using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsLessThanSomeSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyLtSome(property, Query.Criteria);
        }
    }
}