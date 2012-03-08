using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsEqualExpression : SimpleIsExpression
    {
        #region Constructors (1)

        public IsEqualExpression(object value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.Eq(Projections.Property(property), Value);
        }

        #endregion Methods
    }
}