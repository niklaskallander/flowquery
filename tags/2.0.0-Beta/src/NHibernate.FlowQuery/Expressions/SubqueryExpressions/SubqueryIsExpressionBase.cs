using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public abstract class SubqueryIsExpressionBase : SimpleIsExpression
    {
        protected SubqueryIsExpressionBase(IDetachedImmutableFlowQuery query)
            : base(query)
        {
            Query = query;
        }

        protected virtual IDetachedImmutableFlowQuery Query { get; private set; }
    }
}