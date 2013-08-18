using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsLikeExpression : SimpleIsExpression
    {
        public IsLikeExpression(object value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Restrictions.Like(Projections.Property(property), Value);
        }
    }
}