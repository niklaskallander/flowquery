using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanOrEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsLessThanOrEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyLeAll(property, Query.Criteria);
        }

        #endregion Methods
    }
}