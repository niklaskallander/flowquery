using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsEmptyExpression : IsExpression
    {
        protected override ICriterion CompileCore(string property)
        {
            return Restrictions.IsEmpty(property);
        }

        public override ICriterion Compile(string property)
        {
            if (Negated)
            {
                return Restrictions.IsNotEmpty(property);
            }

            return CompileCore(property);
        }

        public virtual ICriterion Compile(DetachedCriteria subquery)
        {
            if (Negated)
            {
                return Subqueries.Exists(subquery);
            }

            return Subqueries.NotExists(subquery);
        }
    }
}