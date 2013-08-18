using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public class Revealer : RevealerBase, IRevealer
    {
        public Revealer(IRevealConvention convention)
            : base(convention)
        { }

        public Revealer()
            : base(new MUnderscoreConvention())
        { }

        public virtual string Reveal<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Reveal(expression, RevealConvention);
        }

        public virtual string Reveal<TEntity>(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return Reveal(property, convention);
        }

        public virtual string Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            return Reveal(alias, expression, RevealConvention);
        }

        public virtual string Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (alias == null)
            {
                throw new ArgumentNullException("alias");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            string aliasName = ExpressionHelper.GetPropertyName(alias.Body);

            return Reveal(aliasName + "." + property, convention);
        }
    }
}