using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsNullExpression : IsExpression
    {
        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.IsNull(Projections.Property(property));
        }

        #endregion Methods
    }
}