using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsGreaterThanSomeSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGtSome(property, Query.Criteria);
        }

        #endregion Methods
    }
}