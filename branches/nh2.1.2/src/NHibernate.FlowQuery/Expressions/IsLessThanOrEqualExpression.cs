using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsLessThanOrEqualExpression : SimpleIsExpression
    {
        #region Constructors (1)

        public IsLessThanOrEqualExpression(object value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.Le(Projections.Property(property), Value);
        }

        #endregion Methods
    }
}