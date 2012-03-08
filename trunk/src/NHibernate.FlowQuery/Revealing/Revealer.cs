using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public class Revealer : RevealerBase, IRevealer
    {
        #region Constructors (2)

        public Revealer(IRevealConvention convention)
            : base(convention)
        { }

        public Revealer()
            : base(new MUnderscoreConvention())
        { }

        #endregion Constructors

        #region Methods (4)

        public virtual string Reveal<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Reveal(expression, RevealConvention);
        }

        protected virtual string Reveal<TEntity>(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return Reveal(property, convention);
        }

        protected virtual string Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            return Reveal(alias, expression, RevealConvention);
        }

        protected virtual string Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention)
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

        #endregion Methods



        #region IRevealer<TEntity> Members

        string IRevealer.Reveal<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Reveal(expression);
        }

        string IRevealer.Reveal<TEntity>(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal(expression, convention);
        }

        string IRevealer.Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            return Reveal(alias, expression);
        }

        string IRevealer.Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal(alias, expression, convention);
        }

        #endregion
    }
}