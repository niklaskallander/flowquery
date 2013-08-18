using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsGreaterThanSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGt(property, Query.Criteria);
        }

        #endregion Methods
    }
}