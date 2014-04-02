using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsGreaterThanExpression : SimpleIsExpression
    {
        public IsGreaterThanExpression(object value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            return Restrictions.Gt(Projections.Property(property), Value);
        }
    }
}