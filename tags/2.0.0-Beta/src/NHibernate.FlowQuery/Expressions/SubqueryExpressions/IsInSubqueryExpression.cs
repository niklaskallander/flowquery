using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsInSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsInSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyIn(property, Query.Criteria);
        }

        #endregion Methods
    }
}