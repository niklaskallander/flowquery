using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsGreaterThanExpression : SimpleIsExpression
    {
        #region Constructors (1)

        public IsGreaterThanExpression(object value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.Gt(Projections.Property(property), Value);
        }

        #endregion Methods
    }
}