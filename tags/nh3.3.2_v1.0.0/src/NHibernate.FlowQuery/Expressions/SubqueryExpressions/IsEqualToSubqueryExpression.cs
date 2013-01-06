using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsEqualToSubqueryExpression<TSource> : SimpleIsExpression
    {
        #region Constructors (1)

        public IsEqualToSubqueryExpression(ISubFlowQuery<TSource> value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyEq(property, (Value as SubFlowQueryImpl<TSource>).Criteria);
        }

        #endregion Methods
    }
}