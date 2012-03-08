using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public class Revealer<TEntity> : Revealer, IRevealer<TEntity>
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

        public virtual string Reveal(Expression<Func<TEntity, object>> expression)
        {
            return Reveal<TEntity>(expression, RevealConvention);
        }

        protected virtual string Reveal(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal<TEntity>(expression, convention);
        }

        protected virtual string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            return Reveal<TEntity>(alias, expression, RevealConvention);
        }

        protected virtual string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal<TEntity>(alias, expression, convention);
        }

        #endregion Methods



        #region IRevealer<TEntity> Members

        string IRevealer<TEntity>.Reveal(Expression<Func<TEntity, object>> expression)
        {
            return Reveal(expression);
        }

        string IRevealer<TEntity>.Reveal(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal(expression, convention);
        }

        string IRevealer<TEntity>.Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            return Reveal(alias, expression);
        }

        string IRevealer<TEntity>.Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal(alias, expression, convention);
        }

        #endregion
    }
}