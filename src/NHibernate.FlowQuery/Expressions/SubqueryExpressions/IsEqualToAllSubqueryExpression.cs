using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyEqAll(property, Query.Criteria);
        }
    }
}