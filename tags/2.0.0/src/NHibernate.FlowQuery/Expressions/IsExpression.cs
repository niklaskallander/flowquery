using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public abstract class IsExpression
    {
        public virtual bool Negate { get; set; }

        public abstract ICriterion Compile(string property);
    }
}