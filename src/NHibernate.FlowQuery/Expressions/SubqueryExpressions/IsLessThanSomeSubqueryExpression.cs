using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsLessThanSomeSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyLtSome(property, Query.Criteria);
        }

        #endregion Methods
    }
}