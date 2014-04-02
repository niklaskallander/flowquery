using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Core.Joins
{
    public interface IJoinBuilder<TSource, TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        TQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias);

        TQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause);

        TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias);

        TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause);

        TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause, IRevealConvention revealConvention);
    }
}