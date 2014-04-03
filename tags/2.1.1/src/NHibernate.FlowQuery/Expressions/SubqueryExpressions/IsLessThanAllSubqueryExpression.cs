using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanAllSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsLessThanAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            return Subqueries.PropertyLtAll(property, Query.Criteria);
        }
    }
}