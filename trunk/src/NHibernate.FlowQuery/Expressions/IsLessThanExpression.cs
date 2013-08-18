using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsLessThanExpression : SimpleIsExpression
    {
        public IsLessThanExpression(object value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Restrictions.Lt(Projections.Property(property), Value);
        }
    }
}