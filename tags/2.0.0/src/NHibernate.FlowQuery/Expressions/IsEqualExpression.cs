using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsEqualExpression : SimpleIsExpression
    {
        public IsEqualExpression(object value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Restrictions.Eq(Projections.Property(property), Value);
        }
    }
}