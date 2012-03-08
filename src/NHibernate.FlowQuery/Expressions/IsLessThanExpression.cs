using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsLessThanExpression : SimpleIsExpression
    {
        #region Constructors (1)

        public IsLessThanExpression(object value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.Lt(Projections.Property(property), Value);
        }

        #endregion Methods
    }
}