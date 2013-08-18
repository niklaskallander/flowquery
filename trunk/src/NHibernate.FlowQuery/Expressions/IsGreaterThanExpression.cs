using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsGreaterThanExpression : SimpleIsExpression
    {
        public IsGreaterThanExpression(object value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Restrictions.Gt(Projections.Property(property), Value);
        }
    }
}