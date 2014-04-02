using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsBetweenExpression : IsExpression
    {
        private object m_Low, m_High;

        public IsBetweenExpression(object low, object high)
        {
            m_Low = low;
            m_High = high;
        }

        protected override ICriterion CompileCore(string property)
        {
            return Restrictions.Between(Projections.Property(property), m_Low, m_High);
        }
    }
}