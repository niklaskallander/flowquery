using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsLessThanExpression : SimpleIsExpression
    {
        public IsLessThanExpression(object value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            return Restrictions.Lt(Projections.Property(property), Value);
        }
    }
}