using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsEqualToSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsEqualToSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyEq(property, Query.Criteria);
        }

        #endregion Methods
    }
}