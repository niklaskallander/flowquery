using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanAllSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsLessThanAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyLtAll(property, Query.Criteria);
        }

        #endregion Methods
    }
}