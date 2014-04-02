using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanOrEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsGreaterThanOrEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            return Subqueries.PropertyGeAll(property, Query.Criteria);
        }
    }
}