using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsNullExpression : IsExpression
    {
        protected override ICriterion CompileCore(string property)
        {
            return Restrictions.IsNull(Projections.Property(property));
        }

        public override ICriterion Compile(string property)
        {
            if (Negated)
            {
                return Restrictions.IsNotNull(Projections.Property(property));
            }

            return CompileCore(property);
        }
    }
}