using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanOrEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsLessThanOrEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            return Subqueries.PropertyLeAll(property, Query.Criteria);
        }
    }
}