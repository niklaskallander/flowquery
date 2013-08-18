using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsGreaterThanOrEqualExpression : SimpleIsExpression
    {
        #region Constructors (1)

        public IsGreaterThanOrEqualExpression(object value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.Ge(Projections.Property(property), Value);
        }

        #endregion Methods
    }
}