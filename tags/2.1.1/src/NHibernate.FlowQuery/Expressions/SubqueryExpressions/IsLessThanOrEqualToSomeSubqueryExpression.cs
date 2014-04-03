using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanOrEqualToSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsLessThanOrEqualToSomeSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            return Subqueries.PropertyLeSome(property, Query.Criteria);
        }
    }
}