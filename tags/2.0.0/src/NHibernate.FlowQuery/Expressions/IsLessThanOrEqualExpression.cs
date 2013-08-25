using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsLessThanOrEqualExpression : SimpleIsExpression
    {
        public IsLessThanOrEqualExpression(object value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Restrictions.Le(Projections.Property(property), Value);
        }
    }
}