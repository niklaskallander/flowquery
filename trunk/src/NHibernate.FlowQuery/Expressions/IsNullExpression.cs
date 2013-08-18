using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsNullExpression : IsExpression
    {
        public override ICriterion Compile(string property)
        {
            return Restrictions.IsNull(Projections.Property(property));
        }
    }
}