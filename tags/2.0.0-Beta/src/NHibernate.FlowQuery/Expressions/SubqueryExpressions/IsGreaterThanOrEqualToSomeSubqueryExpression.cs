using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanOrEqualToSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsGreaterThanOrEqualToSomeSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGeSome(property, Query.Criteria);
        }

        #endregion Methods
    }
}