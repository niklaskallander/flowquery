using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsEqualToAllSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsEqualToAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyEqAll(property, Query.Criteria);
        }

        #endregion Methods
    }
}