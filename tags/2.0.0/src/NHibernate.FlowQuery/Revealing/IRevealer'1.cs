using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public interface IRevealer<TEntity> : IRevealerBase
    {
        string Reveal(Expression<Func<TEntity, object>> expression);

        string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression);

        string Reveal(Expression<Func<TEntity, object>> expression, IRevealConvention convention);

        string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention);
    }
}