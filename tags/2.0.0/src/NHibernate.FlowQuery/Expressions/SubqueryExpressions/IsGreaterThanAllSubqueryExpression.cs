using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanAllSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsGreaterThanAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGtAll(property, Query.Criteria);
        }
    }
}