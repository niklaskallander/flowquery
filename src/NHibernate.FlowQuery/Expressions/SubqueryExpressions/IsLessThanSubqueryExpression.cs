using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsLessThanSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyLt(property, Query.Criteria);
        }

        #endregion Methods
    }
}