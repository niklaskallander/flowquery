using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanAllSubqueryExpression : SubqueryIsExpressionBase
    {
        #region Constructors (1)

        public IsGreaterThanAllSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGtAll(property, Query.Criteria);
        }

        #endregion Methods
    }
}