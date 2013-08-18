using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public interface IRevealer : IRevealerBase
    {
        #region Operations (4)

        string Reveal<TEntity>(Expression<Func<TEntity, object>> expression);

        string Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression);

        string Reveal<TEntity>(Expression<Func<TEntity, object>> expression, IRevealConvention convention);

        string Reveal<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention);

        #endregion Operations
    }
}