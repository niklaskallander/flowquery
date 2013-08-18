using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanOrEqualToSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsGreaterThanOrEqualToSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGe(property, Query.Criteria);
        }

        #endregion Methods
    }
}