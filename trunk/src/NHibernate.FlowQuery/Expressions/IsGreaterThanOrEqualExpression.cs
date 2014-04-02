using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsGreaterThanOrEqualExpression : SimpleIsExpression
    {
        public IsGreaterThanOrEqualExpression(object value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            return Restrictions.Ge(Projections.Property(property), Value);
        }
    }
}