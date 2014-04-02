using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public abstract class IsExpression
    {
        public virtual bool Negated { get; set; }

        protected abstract ICriterion CompileCore(string property);

        public virtual ICriterion Compile(string property)
        {
            ICriterion compiled = CompileCore(property);

            if (Negated)
            {
                return Restrictions.Not(compiled);
            }

            return compiled;
        }
    }
}