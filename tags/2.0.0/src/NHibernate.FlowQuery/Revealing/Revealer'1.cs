using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public class Revealer<TEntity> : Revealer, IRevealer<TEntity>
    {
        public Revealer(IRevealConvention convention)
            : base(convention)
        { }

        public Revealer()
            : base(new MUnderscoreConvention())
        { }

        public virtual string Reveal(Expression<Func<TEntity, object>> expression)
        {
            return Reveal<TEntity>(expression, RevealConvention);
        }

        public virtual string Reveal(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal<TEntity>(expression, convention);
        }

        public virtual string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            return Reveal<TEntity>(alias, expression, RevealConvention);
        }

        public virtual string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal<TEntity>(alias, expression, convention);
        }
    }
}