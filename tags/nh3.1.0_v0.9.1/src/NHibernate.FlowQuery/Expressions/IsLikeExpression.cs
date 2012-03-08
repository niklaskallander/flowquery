using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsLikeExpression : SimpleIsExpression
    {
        #region Constructors (1)

        public IsLikeExpression(object value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.Like(Projections.Property(property), Value);
        }

        #endregion Methods
    }
}