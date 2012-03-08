using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public abstract class IsExpression
    {
        public virtual bool Negate { get; set; }

        #region Methods (1)

        public abstract ICriterion Compile(string property);

        #endregion Methods
    }
}