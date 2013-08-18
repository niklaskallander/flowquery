using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanOrEqualToSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsLessThanOrEqualToSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyLe(property, Query.Criteria);
        }

        #endregion Methods
    }
}