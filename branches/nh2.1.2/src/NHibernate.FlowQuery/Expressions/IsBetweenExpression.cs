using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsBetweenExpression : IsExpression
    {
        #region Fields (1)

        private object m_Low, m_High;

        #endregion Fields

        #region Constructors (1)

        public IsBetweenExpression(object low, object high)
        {
            m_Low = low;
            m_High = high;
        }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Restrictions.Between(Projections.Property(property), m_Low, m_High);
        }

        #endregion Methods
    }
}